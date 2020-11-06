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
 * This is the firmware for the WMOS D1 mini controller. The communication
 * with the ASCOM driver is done via the MQTT protocol.
 * 
 * The dewpoint calculation follows the code example from 
 * https://myscope.net/taupunkttemperatur/, Rortner.
 * 
 * The PID controler was inspired by https://forum.arduino.cc/index.php?topic=433030.0,
 * guntherb.
 * 
 */

#include <FS.h>  
#include <ESP8266WiFi.h>          //https://github.com/esp8266/Arduino
#include <pins_arduino.h>
//needed for library
#include <DNSServer.h>
#include <ESP8266WebServer.h>
#include <WiFiManager.h>          //https://github.com/tzapu/WiFiManager
#include <PubSubClient.h>
#include <ArduinoJson.h>  
#include <stdlib.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BME280.h>

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

char mqtt_server[40];
char mqtt_port[6] = "1883";
String clientId = "StroblCap-";

String DewChannel1_topic = "Astro/StroblCap/ch1";
String DewChannel2_topic = "Astro/StroblCap/ch2";
String DewChannel1OnOff_topic = "Astro/StroblCap/ch1/OnOff";
String DewChannel2OnOff_topic = "Astro/StroblCap/ch2/OnOff";
String DewChannel1Auto_topic = "Astro/StroblCap/ch1/auto";
String DewChannel2Auto_topic = "Astro/StroblCap/ch2/auto";


String DewChannelReport1_topic = "Astro/StroblCap/ch1/state";
String DewChannelReport2_topic = "Astro/StroblCap/ch2/state";
String DewChannelReport1OnOff_topic = "Astro/StroblCap/ch1/stateOnOff";
String DewChannelReport2OnOff_topic = "Astro/StroblCap/ch2/stateOnOff";
String DewChannelReport1Auto_topic = "Astro/StroblCap/ch1/stateAuto";
String DewChannelReport2Auto_topic = "Astro/StroblCap/ch2/stateAuto";

String BME280_topic = "Astro/StroblCap/Env/";

bool mqttconnected = false;

int pwm1;
int pwm2;
int ch1OnOff;
int ch2OnOff;
int ch1Auto;
int ch2Auto;


//flag for saving data
bool shouldSaveConfig = false;
unsigned long ticks = 0;
unsigned long time_now = 0;
int last_state = LOW;

WiFiClient espClient;
PubSubClient client(espClient);

//callback notifying us of the need to save config
void saveConfigCallback () {
  Serial.println("Should save config");
  shouldSaveConfig = true;
}

//gets called when WiFiManager enters configuration mode
void configModeCallback (WiFiManager *myWiFiManager) {
  Serial.println("Entered config mode");
  Serial.println(WiFi.softAPIP());
  Serial.println(myWiFiManager->getConfigPortalSSID());
}

char msg[50];
unsigned long timestamp;
bool msg_sended;
unsigned long timeout;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  pinMode(CHANNEL1,OUTPUT);
  pinMode(CHANNEL2,OUTPUT);
  Serial.println("**********************************************");
  // Check for BME280 environmental sensors:
  
  if (bmeCh1.begin(bme280_ch1addr, &Wire))
  {
    bme280_ch1active = true;
    Serial.println("BME280 for Ch1 found!");
  }
  if (bmeCh2.begin(bme280_ch2addr, &Wire))
  {
    bme280_ch2active = true;
    Serial.println("BME280 for Ch2 found!");
  }

    
  //set led pin as output
  pinMode(BUILTIN_LED, INPUT);
  //read configuration from FS json
  Serial.println("mounting FS...");
  WiFi.hostname("StroblCap");
  if (SPIFFS.begin()) {
    Serial.println("mounted file system");
    if (SPIFFS.exists("/config.json")) {
      //file exists, reading and loading
      Serial.println("reading config file");
      File configFile = SPIFFS.open("/config.json", "r");
      if (configFile) {
        Serial.println("opened config file");
        size_t size = configFile.size();
        // Allocate a buffer to store contents of the file.
        std::unique_ptr<char[]> buf(new char[size]);

        configFile.readBytes(buf.get(), size);
        DynamicJsonBuffer jsonBuffer;
        JsonObject& json = jsonBuffer.parseObject(buf.get());
        json.printTo(Serial);
        if (json.success()) {
          Serial.println("\nparsed json");

          strcpy(mqtt_server, json["mqtt_server"]);
          strcpy(mqtt_port, json["mqtt_port"]);

        } else {
          Serial.println("failed to load json config");
        }
        configFile.close();
      }
    }
  } else {
    Serial.println("failed to mount FS");
  }
  //end read
  // Add mac address to the mqtt client name
  clientId += WiFi.macAddress();


  // The extra parameters to be configured (can be either global or just in the setup)
  // After connecting, parameter.getValue() will get you the configured value
  // id/name placeholder/prompt default length
  WiFiManagerParameter custom_mqtt_server("server", "mqtt server", mqtt_server, 40);
  WiFiManagerParameter custom_mqtt_port("port", "mqtt port", mqtt_port, 6);
  
  //WiFiManager
  //Local intialization. Once its business is done, there is no need to keep it around
  WiFiManager wifiManager;
  //set callback that gets called when connecting to previous WiFi fails, and enters Access Point mode
  wifiManager.setAPCallback(configModeCallback);
   //set config save notify callback
  wifiManager.setSaveConfigCallback(saveConfigCallback);

  //add all your parameters here
  wifiManager.addParameter(&custom_mqtt_server);
  wifiManager.addParameter(&custom_mqtt_port);

  //fetches ssid and pass and tries to connect
  //if it does not connect it starts an access point with the specified name
  //here  "AutoConnectAP"
  //and goes into a blocking loop awaiting configuration
  if (!wifiManager.autoConnect()) {
    Serial.println("failed to connect and hit timeout");
    //reset and try again, or maybe put it to deep sleep
    ESP.reset();
    delay(1000);
  }

  //if you get here you have connected to the WiFi
  Serial.println("WiFi ok. Connect to mqtt broker...");
  //read updated parameters
  strcpy(mqtt_server, custom_mqtt_server.getValue());
  strcpy(mqtt_port, custom_mqtt_port.getValue());
  //save the custom parameters to FS
  if (shouldSaveConfig) {
    Serial.println("saving config");
    DynamicJsonBuffer jsonBuffer;
    JsonObject& json = jsonBuffer.createObject();
    json["mqtt_server"] = mqtt_server;
    json["mqtt_port"] = mqtt_port;

    File configFile = SPIFFS.open("/config.json", "w");
    if (!configFile) {
      Serial.println("failed to open config file for writing");
    }

    json.printTo(Serial);
    json.printTo(configFile);
    configFile.close();
    //end save
  }

  
  Serial.println("local ip");
  Serial.println(WiFi.localIP());

  int mqttport = atoi(mqtt_port);
  String mqttsrv = mqtt_server;
  IPAddress addr;
  addr.fromString(mqttsrv);
  Serial.println("Connect to broker:");
  Serial.println(addr);
  Serial.println(mqttport);
  
  client.setServer(addr, mqttport);
  client.setCallback(callback);
  timeout = 60 * 1000;  
}

void callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Message arrived [");
  Serial.print(topic);
  Serial.print("] ");
  if(length == 1)
    Serial.println((int)payload[0]);
  Serial.println();
  char pubPayload[1];
  if(strcmp(topic, DewChannel1_topic.c_str()) == 0)
  {
    Serial.println("Channel 1:");
    if(length == 1) 
    {
      pwm1 = (int)payload[0];
      pubPayload[0] = (int)pwm1;
      client.publish(DewChannelReport1_topic.c_str(), pubPayload);
      pwm1 -= 100;
    }
  }
  if(strcmp(topic, DewChannel2_topic.c_str()) == 0)
  {
    Serial.println("Channel 2:");
    if(length == 1) 
    {
      pwm2 = (int)payload[0];
      pubPayload[0] = (int)pwm2;
      client.publish(DewChannelReport2_topic.c_str(), pubPayload);
      pwm2 -= 100;
    }
  }
  if(strcmp(topic, DewChannel1OnOff_topic.c_str()) == 0)
  {
    Serial.println("Channel 1 OnOff:");
    if(length == 1) 
    {
      ch1OnOff = (int)payload[0]-1;
      pubPayload[0] = (int)ch1OnOff+1;
      client.publish(DewChannelReport1OnOff_topic.c_str(), pubPayload);
    }
  }
  if(strcmp(topic, DewChannel2OnOff_topic.c_str()) == 0)
  {
    Serial.println("Channel 2 OnOff:");
    if(length == 1) 
    {
      ch2OnOff = (int)payload[0]-1;
      pubPayload[0] = (int)ch2OnOff+1;
      client.publish(DewChannelReport2OnOff_topic.c_str(), pubPayload);
    }
  }
  if(strcmp(topic, DewChannel1Auto_topic.c_str()) == 0)
  {
    Serial.println("Channel 1 Auto:");
    if(length == 1) 
    {
      ch1Auto = (int)payload[0]-1;
      pubPayload[0] = (int)ch1Auto+1;
      client.publish(DewChannelReport1Auto_topic.c_str(), pubPayload);
    }
  }
  if(strcmp(topic, DewChannel2Auto_topic.c_str()) == 0)
  {
    Serial.println("Channel 2 Auto:");
    if(length == 1) 
    {
      ch2Auto = (int)payload[0]-1;
      pubPayload[0] = (int)ch2Auto+1;
      client.publish(DewChannelReport2Auto_topic.c_str(), pubPayload);
    }
  }
  SetPwmOnChannel();
}

void reconnect() {
  // Loop until we're reconnected
  if (!client.connected()) {
    Serial.print("Attempting MQTT connection...");
    // Attempt to connect
    if (client.connect(clientId.c_str())) {
      Serial.println("connected");
      client.subscribe(DewChannel1_topic.c_str());
      client.subscribe(DewChannel2_topic.c_str());
      client.subscribe(DewChannel1OnOff_topic.c_str());
      client.subscribe(DewChannel2OnOff_topic.c_str());
      client.subscribe(DewChannel1Auto_topic.c_str());
      client.subscribe(DewChannel2Auto_topic.c_str());
      mqttconnected = true;
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
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
  
  Serial.print("Temperature = ");
  Serial.print(temp);
  Serial.println(" *C");

  Serial.print(pres);
  Serial.println(" hPa");

  Serial.print("Humidity = ");
  Serial.print(hum);
  Serial.println(" %");

  Serial.print("Dewpoint = ");
  Serial.print(dew);
  Serial.println(" *C");

  if(mqttconnected) PublishEnvironment(ch ,temp, hum, pres);
  if(ch == Channel1) SetPower(ch, temp, dew);
  if(ch == Channel2) SetPower(ch, temp, dew);
}

void PublishEnvironment(String ch, double temp, double hum, double pres)
{
  double dewpoint = DewPoint(temp, hum);

  String temp_topic = BME280_topic + ch + "/Temp";
  String pres_topic = BME280_topic + ch + "/Pressure";
  String hum_topic = BME280_topic + ch + "/Humidity";
  String dew_topic = BME280_topic + ch + "/Dewpoint";

  String tempPayload = String(temp);
  String presPayload = String(pres);
  String humPayload = String(hum);
  String dewPayload = String(dewpoint);

  client.publish(temp_topic.c_str(), tempPayload.c_str());
  client.publish(pres_topic.c_str(), presPayload.c_str());
  client.publish(hum_topic.c_str(), humPayload.c_str());
  client.publish(dew_topic.c_str(), dewPayload.c_str());
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
    Serial.println("Calculate power Channel 1 ....");
    char payload[2];
    if(!ch1OnOff)
    {
      payload[0] = 100; // means 0...
      payload[1] = 0;
      client.publish(DewChannelReport1_topic.c_str(), payload);
    }
    else
    {
      if(ch1Auto)
      {
        double out = PID(dewPointDiff, temp-dewpoint, KpCh1, KiCh1, &outICh1);
        if(out > 100.0) out = 100.0;
        if(out < 0.0) out = 0.0;
        pwm1 = (int)out;
        Serial.print("PID OUT: ");
        Serial.println(out);
        Serial.print("Diff: ");
        Serial.println(temp-dewpoint);
      }
      payload[0] = pwm1 + 100;
      payload[1] = 0;
      client.publish(DewChannelReport1_topic.c_str(), payload);  
    }
  }

  if(ch == Channel2)
  {
    Serial.println("Calculate power Channel 2 ....");
    char payload[2];
    if(!ch2OnOff)
    {
      payload[0] = 100; // means 0...
      payload[1] = 0;
      client.publish(DewChannelReport2_topic.c_str(), payload);
    }
    else
    {
      if(ch2Auto)
      {
        double out = PID(dewPointDiff, temp-dewpoint, KpCh2, KiCh2, &outICh2);
        if(out > 100.0) out = 100.0;
        if(out < 0.0) out = 0.0;
        pwm1 = (int)out;
        Serial.print("PID OUT: ");
        Serial.println(out);
        Serial.print("Diff: ");
        Serial.println(temp-dewpoint);
      }
      payload[0] = pwm2 + 100;
      payload[1] = 0;
      client.publish(DewChannelReport2_topic.c_str(), payload);
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
  Serial.print("Set pwm channel 1 to ");
  Serial.println(localPwm1);
  Serial.print("Set pwm channel 2 to ");
  Serial.println(localPwm2);   
}

void loop() {
  if (!client.connected()) {
    mqttconnected = false;
    reconnect();
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
  
  client.loop();
}
