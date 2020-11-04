#include <FS.h>  
#include <ESP8266WiFi.h>          //https://github.com/esp8266/Arduino

//needed for library
#include <DNSServer.h>
#include <ESP8266WebServer.h>
#include <WiFiManager.h>          //https://github.com/tzapu/WiFiManager
#include <PubSubClient.h>
#include <ArduinoJson.h>  
#include <stdlib.h>


/*
 * var t = 25; // Luft-Temperatur (°C)
var r = 40; // relative Luftfeuchtigkeit (%)
var tp = taupunkt(t, r);
 
// Taupunkt Berechnung
function taupunkt(t, r) {
  // Konstante
  var mw = 18.016; // Molekulargewicht des Wasserdampfes (kg/kmol)
  var gk = 8214.3; // universelle Gaskonstante (J/(kmol*K))
  var t0 = 273.15; // Absolute Temperatur von 0 °C (Kelvin)
  var tk = t + t0; // Temperatur in Kelvin
 
  var a, b;
  if (t >= 0) {
    a = 7.5;
    b = 237.3;
  } else if (t < 0) {
    a = 7.6;
    b = 240.7;
  }
 
  // Sättigungsdampfdruck (hPa)
  var sdd = 6.1078 * Math.pow(10, (a*t)/(b+t));
 
  // Dampfdruck (hPa)
  var dd = sdd * (r/100);
 
  // Wasserdampfdichte bzw. absolute Feuchte (g/m3)
  af = Math.pow(10,5) * mw/gk * dd/tk;
 
  // v-Parameter
  v = Math.log10(dd/6.1078);
 
  // Taupunkttemperatur (°C)
  td = (b*v) / (a-v);
  return { td: td, af: af, dd: dd };  
}
 */
 
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
}

void reconnect() {
  // Loop until we're reconnected
  while (!client.connected()) {
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
    } else {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}

void loop() {
  if (!client.connected()) {
    reconnect();
  }
  client.loop();
}
