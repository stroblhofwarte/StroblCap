/* 
 * This file is part of the StroblCap projekt (https://astro.stroblhof-oberrohrbach.de)
 * Copyright (c) 2020 Othmar Ehrhardt
 * 
 * This program is free software: you can redistribute it and/or modify  
 * it under the terms of the GNU General Public License as published by  
 * the Free Software Foundation, version 3.
 *
 * This program is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU 
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License 
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 * 
 * This is the firmware for the WMOS D1 mini controller. 
 * It use the serial interface.
 * 
 * The dewpoint calculation follows the code example from 
 * https://myscope.net/taupunkttemperatur/, Rortner.
 * 
 * The PID controler was inspired by https://forum.arduino.cc/index.php?topic=433030.0,
 * guntherb.
 * 
 * 
 * Commands (colon is part of the command):
 * ID:            Sent back the device type id with STROBLCAP#
 * S1nnn:         Set power for channel 1 to nnn, where nnn is 000 - 100 [%]
 * S2nnn:         Set power for channel 2 to nnn, return 1# when set, otherwise 0# or nothing.
 * E1:            Enable channel 1. 1# if ok.
 * E2:            Enable channel 2. 1# if ok.
 * D1:            Disable channel 1
 * D2:            Disable channel 2
 * A1:            Set channel 1 to automatic mode. 1# if ok, 0# when no sensor was found for this channel.
 * A2:            Set channel 2 to automatic mode. 1# if ok, 0# when no sensor.
 * M1:            Set channel 1 to manual mode.
 * M2:            Set channel 2 to manual mode.
 * G1:            Get sensor data channel 1: "t;h;d;p#", where
 *                            t: Temperature * 100,
 *                            h: Humidity * 100,
 *                            d: Dewpoint * 100,
 *                            p: Pressure * 10
 * G2:            Get sensor data channel 2: "t;h;d;p#", where
 *                            t: Temperature * 100,
 *                            h: Humidity * 100,
 *                            d: Dewpoint * 100,
 *                            p: Pressure * 10
 * P1:            Get power value channel 1, nnn#
 * P2:            Get power value channel 2, nnn#
 */

#include <ESP8266WiFi.h>
#include <pins_arduino.h>
//needed for library
#include <stdlib.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BME280.h>
#include <strings.h>

// Hardware settings
int CHANNEL1 = D5;
int CHANNEL2 = D6;

double pwmFactor = PWMRANGE / 100.0;

Adafruit_BME280 bmeCh1; // I2C
Adafruit_BME280 bmeCh2; // I2C
byte bme280_ch1addr = 0x76;
byte bme280_ch2addr = 0x77;
bool bme280_ch1active = false;
bool bme280_ch2active = false;

const String Channel1 = "ch1";
const String Channel2 = "ch2";


int pwm1;
int pwm2;
int ch1OnOff;
int ch2OnOff;
int ch1Auto;
int ch2Auto;

unsigned long timestamp;

int serialDataReady = 0;
String serialData;
String serBuf;

void setup() {
  WiFi.forceSleepBegin();
  // put your setup code here, to run once:
  Serial.begin(115200);
  pinMode(CHANNEL1,OUTPUT);
  pinMode(CHANNEL2,OUTPUT);
  pinMode(LED_BUILTIN, OUTPUT);
  digitalWrite(LED_BUILTIN, HIGH);
  serBuf = "";
  serialData = "";
  serialDataReady = 0;

  // Check for BME280 environmental sensors:
  
  if (bmeCh1.begin(bme280_ch1addr, &Wire))
  {
    bme280_ch1active = true;
  }
  if (bmeCh2.begin(bme280_ch2addr, &Wire))
  {
    bme280_ch2active = true;
  }

    
  //set led pin as output
  pinMode(BUILTIN_LED, INPUT);
}

double DewPoint(double temp, double humidity)
{
  // Konstante
  double mw = 18.016; // Molekulargewicht des Wasserdampfes (kg/kmol)
  double gk = 8214.3; // universelle Gaskonstante (J/(kmol*K))
  double t0 = 273.15; // Absolute Temperatur von 0 °C (Kelvin)
  double tk = temp + t0; // Temperatur in Kelvin
 
  double a, b;
  if (temp >= 0) {
    a = 7.5;
    b = 237.3;
  } else if (temp < 0) {
    a = 7.6;
    b = 240.7;
  }
 
  // Sättigungsdampfdruck (hPa)
  double sdd = 6.1078 * pow(10, (a*temp)/(b+temp));
 
  // Dampfdruck (hPa)
  double dd = sdd * (humidity/100);
 
  // Wasserdampfdichte bzw. absolute Feuchte (g/m3)
  double af = pow(10,5) * mw/gk * dd/tk;
 
  // v-Parameter
  double v = log10(dd/6.1078);
 
  // Taupunkttemperatur (°C)
  double td = (b*v) / (a-v);
  return td;
}

void EnvironmentReadout(Adafruit_BME280 bme, String ch)
{
  double temp = bme.readTemperature();
  double hum = bme.readHumidity();
  double pres = bme.readPressure() / 100.0F;
  double dew = DewPoint(temp, hum);

  if(ch == Channel1) SetPower(ch, temp, dew);
  if(ch == Channel2) SetPower(ch, temp, dew);
}

double dewPointDiff = 5.0;

double KpCh1 = 50;
double KiCh1 = 0.01;
double outICh1 = 0.0;

double KpCh2 = 50;
double KiCh2 = 0.01;
double outICh2 = 0.0;

double PID(double soll, double ist, double Kp, double Ki, double *outI)
{
  double outP = (soll - ist ) * Kp;
  *outI += (soll - ist) * Ki;   // Ki = 0,2
  if(*outI < 0.0) *outI = 0.0;
  double out = outP + *outI;
  return out;
}

void SetPower(String ch, double temp, double dewpoint)
{
  if(ch == Channel1)
  {
    char payload[2];
    if(!ch1OnOff)
    {

    }
    else
    {
      if(ch1Auto)
      {
        double out = PID(dewPointDiff, temp-dewpoint, KpCh1, KiCh1, &outICh1);
        if(out > 100.0) out = 100.0;
        if(out < 0.0) out = 0.0;
        pwm1 = (int)out;
      }
    }
  }

  if(ch == Channel2)
  {
    char payload[2];
    if(!ch2OnOff)
    {
      
    }
    else
    {
      if(ch2Auto)
      {
        double out = PID(dewPointDiff, temp-dewpoint, KpCh2, KiCh2, &outICh2);
        if(out > 100.0) out = 100.0;
        if(out < 0.0) out = 0.0;
        pwm1 = (int)out;
      }
    }
  }
}

void SetPwmOnChannel()
{
  int localPwm1 = (int)((double)pwm1 * pwmFactor);
  int localPwm2 = (int)((double)pwm2 * pwmFactor);
  
  if(!ch1OnOff) localPwm1 = 0;
  analogWrite(CHANNEL1, localPwm1);
  if(!ch2OnOff) localPwm2 = 0;
  analogWrite(CHANNEL2, localPwm2); 
}

void Greeting()
{
  Serial.print("STROBLCAP#");
  Serial.flush();
}

void VisualAck()
{
  digitalWrite(LED_BUILTIN, LOW);
  delay(100); 
  digitalWrite(LED_BUILTIN, HIGH);
}

void Cmd_S(String data, int ch)
{
  String value = data.substring(2,5);
  if(ch == 1) pwm1 = value.toInt();
  else if(ch == 2) pwm2 = value.toInt();
  else
  {
    Serial.print("0#");
    return;
  }
  SetPwmOnChannel();
  Serial.print("1#");
}

void Cmd_ED(int ch, bool val)
{
  if(ch == 1) ch1OnOff = val;
  else if(ch == 2) ch2OnOff = val;
  else
  {
    Serial.print("0#");
    return;
  }
  Serial.print("1#");  
}

void Cmd_AM(int ch, bool val)
{
  if(ch == 1) ch1Auto = val;
  else if(ch == 2) ch2Auto = val;
  else
  {
    Serial.print("0#");
    return;
  }
  Serial.print("1#");  
}

void Cmd_G(int ch)
{
  char sensorstring[20];
  if(ch == 1)
  {
    if(bme280_ch1active)
    {
      double temp = bmeCh1.readTemperature();
      double hum = bmeCh1.readHumidity();
      double pres = bmeCh1.readPressure() / 100.0F;
      double dew = DewPoint(temp, hum);
      sprintf(sensorstring, "%d;%d;%d;%d#", (int)(temp * 100), (int)(hum * 100), (int)(dew * 100), (int)(pres * 10));
    }
    else
      sprintf(sensorstring, "0;0;0;0#");
    Serial.print(sensorstring);
  }
  if(ch == 2)
  {
    if(bme280_ch2active)
    {
      double temp = bmeCh2.readTemperature();
      double hum = bmeCh2.readHumidity();
      double pres = bmeCh2.readPressure() / 100.0F;
      double dew = DewPoint(temp, hum);
      sprintf(sensorstring, "%d;%d;%d;%d#", (int)(temp * 100), (int)(hum * 100), (int)(dew * 100), (int)(pres * 10));
    }
    else
      sprintf(sensorstring, "0;0;0;0#");
    Serial.print(sensorstring);
  }
}

void Cmd_P(int ch)
{
  if(ch == 1)
  {
    if(ch1OnOff)
      Serial.print(pwm1);
    else 
      Serial.print(0);
    Serial.print("#");
  }
  if(ch == 2)
  {
    if(ch2OnOff)
      Serial.print(pwm2);
    else 
      Serial.print(0);
    Serial.print("#");
  }
}

void process(String data)
{
  if(data == "ID:") Greeting();
  else if(data.startsWith("S1")) Cmd_S(data, 1);
  else if(data.startsWith("S2")) Cmd_S(data, 2);
  else if(data == "E1:") Cmd_ED(1, true);
  else if(data == "E2:") Cmd_ED(2, true);
  else if(data == "D1:") Cmd_ED(1, false);
  else if(data == "D2:") Cmd_ED(2, false);
  else if(data == "A1:") Cmd_AM(1, true);
  else if(data == "A2:") Cmd_AM(2, true);
  else if(data == "M1:") Cmd_AM(1, false);
  else if(data == "M2:") Cmd_AM(2, false);
  else if(data == "G1:") Cmd_G(1);
  else if(data == "G2:") Cmd_G(2);
  else if(data == "P1:") Cmd_P(1);
  else if(data == "P2:") Cmd_P(2);
  else
  {
    Serial.print("0#");
  }
}

void loop() { 
  // if there's any serial available, read it:
  if(Serial.available() > 0 && serialDataReady == 0) 
  {
    char c = Serial.read();
    if(c != '\n')
    {
      if(c == ':')
      {
        // Data line is complete
        serBuf += String(c);
        serialData = serBuf;
        serBuf = "";
        serialDataReady = 1;
      }
      else
        serBuf += String(c);
    }
  }
  if(serialDataReady == 1)
  {
    serialDataReady = 0;
    VisualAck();
    process(serialData);
  }
  
  if( millis() - timestamp >= 10000)
  { 
    // Execute this all 10 seconds    
    timestamp += 10000; 
    if(bme280_ch1active)
      EnvironmentReadout(bmeCh1, Channel1);
    if(bme280_ch2active)
      EnvironmentReadout(bmeCh2, Channel2);
    SetPwmOnChannel();
  }
}
