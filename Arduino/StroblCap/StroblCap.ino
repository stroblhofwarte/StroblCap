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
 * E1:            Enable channel 1. 1# if ok.
 * E2:            Enable channel 2. 1# if ok.
 * D1:            Disable channel 1
 * D2:            Disable channel 2
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
 * T:             Get thermistor temp: "t#", where
 *                            t: Temperature * 100
 * TN:            Set nominal value of the thermistor [*1, Ohm]
 * TT:            Set nominal temperature of the thermistor [*1, °C]
 * TC:            Set thermistor beta coeffizient [*1]
 * 
 * Thermistor: Temp of dew heater 1
 * Sensor Ch1: Temp of dew heater 2
 * Sensor Ch2: Outside temp and humifdity
 */


#include <ESP8266WiFi.h>
#include <pins_arduino.h>
//needed for library
#include <stdlib.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BME280.h>
#include <strings.h>

#define DEBUG 1

// Hardware settings
int CHANNEL1 = D5;
int CHANNEL2 = D6;
int THERMISTOR = A0;

Adafruit_BME280 bmeCh1; // I2C
Adafruit_BME280 bmeCh2; // I2C
byte bme280_ch1addr = 0x76;
byte bme280_ch2addr = 0x77;
bool bme280_ch1active = false;
bool bme280_ch2active = false;

const String Channel1 = "ch1";
const String Channel2 = "ch2";

int ch1OnOff = 1;
int ch2OnOff = 1;

int thermistorNominal = 10000;
int thermistorTempNominal = 25;
int bCoefficient = 3977;
unsigned long timestamp;

int serialDataReady = 0;
String serialData;
String serBuf;

void setup() {
  
  WiFi.forceSleepBegin();
  // put your setup code here, to run once:
  Serial.begin(115200);
  //analogWriteFreq(1000);
  pinMode(CHANNEL1,OUTPUT);
  digitalWrite(CHANNEL1,LOW);
  pinMode(CHANNEL2,OUTPUT);
  digitalWrite(CHANNEL2,LOW);
  pinMode(THERMISTOR, INPUT); 
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

void EnvironmentReadout(String ch)
{
  if(ch == Channel1)
  {
    double temp = bmeCh1.readTemperature();
    double hum = bmeCh1.readHumidity();
    double pres = bmeCh1.readPressure() / 100.0F;
    double dew = DewPoint(temp, hum);
    double thermistorTemp = ThermistorReadout();
  }
  if(ch == Channel2)
  {
    double temp = bmeCh2.readTemperature();
    double hum = bmeCh2.readHumidity();
    double pres = bmeCh2.readPressure() / 100.0F;
    double dew = DewPoint(temp, hum);
  }
}

// Thermistor calculation taken from http://www.scynd.de/tutorials/arduino-tutorials/5-sensoren/5-1-temperatur-mit-10k%CF%89-ntc.html
#define VALUES 5
#define FIXED_RESISTOR 10000 // in Ohm
double ThermistorReadout()
{
  int values[VALUES];   
  for (int i=0; i < VALUES; i++)
  {
    values[i] = analogRead(THERMISTOR);
    delay(10);
  }
   
  // Average all values:
  int average = 0;
  for (int i=0; i < VALUES; i++)
  {
    average += values[i];
  }
  average /= VALUES;

  // Calculate resitance in ohm
  double r =  (double)FIXED_RESISTOR *((1023.0 / (double)average ) - 1.0);

  // Steinhard calculation
  double temp = r / (double)thermistorNominal;      // (R/Ro)
  temp = log(temp);                               // ln(R/Ro)
  temp /= (double)bCoefficient;                           // 1/B * ln(R/Ro)
  temp += 1.0 / ((double)thermistorTempNominal + 273.15); // + (1/To)
  temp = 1.0 / temp;                              // invert
  temp -= 273.15;                                 // to °C
  return temp;
}

double agres1 = 1;
double agres2 = 1;

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

long Extract(String cmdid, String cmdstring)
{
  cmdstring.remove(0, cmdid.length());
  cmdstring.replace(':', ' ');
  cmdstring.trim();
  char tarray[32];
  cmdstring.toCharArray(tarray, sizeof(tarray));
  long steps = atol(tarray);
  return steps;
}

float FloatExtract(String cmdid, String cmdstring)
{
  cmdstring.remove(0, cmdid.length());
  cmdstring.replace(':', ' ');
  cmdstring.trim();
  char tarray[32];
  cmdstring.toCharArray(tarray, sizeof(tarray));
  float steps = atof(tarray);
  return steps;
}

void Cmd_TN(String data)
{
  thermistorNominal = Extract("TN",data);
  Serial.print("1#");
}

void Cmd_TT(String data)
{
  thermistorTempNominal = Extract("TT",data);
  Serial.print("1#");
}

void Cmd_TC(String data)
{
  bCoefficient = Extract("TC",data);
  Serial.println(bCoefficient);
  Serial.print("1#");
}

void Cmd_ED(int ch, bool val)
{
  if(ch == 1) 
  {
    ch1OnOff = val;
    if(val)
      digitalWrite(CHANNEL1, HIGH);
    else
      digitalWrite(CHANNEL1, LOW);
  }
  else if(ch == 2) 
  {
    ch2OnOff = val;
    if(val)
      digitalWrite(CHANNEL2, HIGH);
    else
      digitalWrite(CHANNEL2, LOW);
  }
  else
  {
    Serial.print("1#");
    return;
  }
  if(ch == 1 && bme280_ch1active)
  {
    Serial.print("1#");  
    return;
  }
  if(ch == 2 && bme280_ch2active)
  {
    Serial.print("1#"); 
    return; 
  }
  Serial.print("1#");  
    return;
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

void Cmd_T(double temp)
{
  Serial.print((int)(temp * 100));
  Serial.print("#");
}

void process(String data)
{
  if(data == "ID:") Greeting();
  else if(data == "E1:") Cmd_ED(1, true);
  else if(data == "E2:") Cmd_ED(2, true);
  else if(data == "D1:") Cmd_ED(1, false);
  else if(data == "D2:") Cmd_ED(2, false);
  else if(data == "G1:") Cmd_G(1);
  else if(data == "G2:") Cmd_G(2);
  else if(data.startsWith("TN")) Cmd_TN(data);
  else if(data.startsWith("TC")) Cmd_TC(data);
  else if(data.startsWith("TT")) Cmd_TT(data);
  else if(data == "T:") Cmd_T(ThermistorReadout());
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
      EnvironmentReadout(Channel1);
    if(bme280_ch2active)
      EnvironmentReadout(Channel2);
  }
}
