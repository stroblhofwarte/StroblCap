// Device information:
// Shorts
// ======
//
// LUP: Undervoltage protection (in V)
// OUP: Overvoltage protection (in V)
// OCP: Overcurrent protection (in A)
// OPP: Over power protection (in W)
// OAH: Maximum Capacity (in Ah)
// OPH: Maximum energy before shutdown (in Wh)
// OHP: Running time protection
// OTP: Overtemperature protection
// ETP: External temperature protection
// ADD: Modbus address (115200 baud)
// POn: Power on state
// FET: Rotary encoder operation: Set Voltage, set current, off
//
// Register (Word):
// ================
// 1: Current (Set)
// 2: Voltage (Set)
// 3: Current
// 4: Power (Watt)
// 5: Input voltage
// 6: Ah
//
// 8: Wh
// 10: Houres running On
// 11: Minutes running On
// 11: Seconds running On
// 15: Lock mode (1: Lock)
// 17: Mode (CV or CC, CC = 1)????
// 18: On/Off (1: On)

#define DEVICE_IDENTIFICATION "SWITCH"
#define CMD_IDENTIFICATION "ID"
#define CMD_REL_1_ON "R1ON"       // Relais 1 on 
#define CMD_REL_1_OFF "R1OFF"     // Relais 1 off
#define CMD_REL_2_ON "R2ON"       // Relais 2 on
#define CMD_REL_2_OFF "R2OFF"     // Relais 2 off
#define CMD_PWR_ON "SPWON"        // Power out on (regulated power source)
#define CMD_PWR_OFF "SPWOFF"      // Power out off (regulated power source)
#define CMD_PWR_SET "PWR"         // Set the output power of regulated power source in Watt
#define CMD_READ_VOLT "VOLT"      // Read the regulated power source output voltage (in V)
#define CMD_READ_CURRENT "AMP"    // Read the regulated power source output current (in A)
#define CMD_READ_AH "AH"          // Read the regulated power source amper hours (in Ah)
#define CMD_READ_WH "WH"          // Read the regulated power source watt hours (in Wh)
#define CMD_READ_WATT "WATT"      // Read the regulated power source current watt reading (in W)
#define CMD_READ_INP_VOLT "INVO"  // Read the input voltage of the regulated power source (in Volt)
#define CMD_INTERNAL_TEMP "IT"    // Read the internal temperatur sensor
#define CMD_INTERNAL_HUM "IH"     // Read the internal humidity sensor
#define CMD_EXTERNAL_TEMP "ET"    // Read the external temperature sensor when connected
#define CMD_EXTERNAL_HUM "EH"     // Read the external humidity sensor when connected

#include <Arduino.h>
#include <ModbusMaster.h>
#include <SoftwareSerial.h>

#define REL1_PIN 6
#define REL2_PIN 7

// SDA AD4
// SCL AD5

double volatile g_volt;
double volatile g_current;
double volatile g_power;
double volatile g_Ah;
double volatile g_Wh;
double volatile g_inputVoltage;
bool volatile g_powerstate;

double volatile g_targetPower = 2.5; // Watt
double volatile g_targetVoltage = 12.0; // Volt
double volatile g_targetCurrent = g_targetPower / g_targetVoltage;
double volatile g_stepCorr = 0.01; // Ampere

String g_command = "";
bool g_commandComplete = false;

SoftwareSerial mySerial(3, 4); // RX, TX

// instantiate ModbusMaster object
ModbusMaster node;

void setup()
{
  Serial.begin(19200);
  mySerial.begin(115200);

  pinMode(REL1_PIN,OUTPUT);
  pinMode(REL2_PIN,OUTPUT);
  // Init Modbus for Slave 1:
  node.begin(1, mySerial);
}

void SetVoltAndCurrent(float fVolt, float fCurrent)
{
  uint8_t result;
  
  word wVolt = fVolt * 100;
  word wAmpere = fCurrent * 100;
 
  node.setTransmitBuffer(0, wVolt);
  node.setTransmitBuffer(1, wAmpere);
  result = node.writeMultipleRegisters(0, 2);
}

void ModbusRefresh()
{
  uint16_t data[29];
  uint8_t j, result;
  result = node.readHoldingRegisters(1, 29);
  // do something with data if read is successful
  if (result == node.ku8MBSuccess)
  {
    
    for (j = 0; j < 29; j++)
    {
      data[j] = node.getResponseBuffer(j);
    }
    g_volt = data[1]; g_volt = g_volt / 100.0;
    g_current = data[0]; g_current = g_current /100.0;
    g_power = data[3]; g_power = g_power /10.0;
    g_Ah = data[5]; g_Ah = g_Ah /1000.0;
    g_Wh = data[7]; g_Wh = g_Wh /1000.0;
    g_inputVoltage = data[4]; g_inputVoltage = g_inputVoltage / 100.0;
    if(data[17] == 1) g_powerstate = true;
    else g_powerstate = false;
  }
}

void PowerOnOff(bool state)
{
  uint8_t result;
  if(state)
    result = node.writeSingleRegister(18,1);
  else
    result = node.writeSingleRegister(18,0);
}

float Extract(String cmdid, String cmdstring)
{
  cmdstring.remove(0, cmdid.length());
  cmdstring.replace(':', ' ');
  cmdstring.trim();
  return cmdstring.toFloat();
}


void Dispatcher()
{
  if(g_command.startsWith(CMD_IDENTIFICATION))
  {
    Serial.print(DEVICE_IDENTIFICATION);
    Serial.print('#');
  }
  else if(g_command.startsWith(CMD_REL_1_ON))
  {
    digitalWrite(REL1_PIN,HIGH);
    Serial.print("1#");
  }
  else if(g_command.startsWith(CMD_REL_1_OFF))
  {
    digitalWrite(REL1_PIN,LOW);
    Serial.print("1#");
  }
  else if(g_command.startsWith(CMD_REL_2_ON))
  {
    digitalWrite(REL2_PIN,HIGH);
    Serial.print("1#");
  }
  else if(g_command.startsWith(CMD_REL_2_OFF))
  {
    digitalWrite(REL2_PIN,LOW);
    Serial.print("1#");
  }
  else if(g_command.startsWith(CMD_PWR_ON))
  {
    PowerOnOff(true);
    Serial.print("1#");
  }
  else if(g_command.startsWith(CMD_PWR_OFF))
  {
    PowerOnOff(false);
    Serial.print("1#");
  }
  else if(g_command.startsWith(CMD_PWR_SET))
  {
    float val = Extract(CMD_PWR_SET, g_command);
    g_targetPower = val;
    Serial.print("1#");
  }
  else if(g_command.startsWith(CMD_READ_INP_VOLT))
  {
    Serial.print(g_inputVoltage);
    Serial.print("#");
  }
  else if(g_command.startsWith(CMD_READ_VOLT))
  {
    Serial.print(g_volt);
    Serial.print("#");
  }
  else if(g_command.startsWith(CMD_READ_CURRENT))
  {
    Serial.print(g_current);
    Serial.print("#");
  }
  else if(g_command.startsWith(CMD_READ_AH))
  {
    Serial.print(g_Ah);
    Serial.print("#");
  }
  else if(g_command.startsWith(CMD_READ_WH))
  {
    Serial.print(g_Wh);
    Serial.print("#");
  }
  else if(g_command.startsWith(CMD_READ_WATT))
  {
    Serial.print(g_power);
    Serial.print("#");
  }
 
  g_command = "";
  g_commandComplete = false;
}

void loop()
{
  if(g_commandComplete)
  {
    Dispatcher();
  }
  delay(250);
  ModbusRefresh();
 
  if(g_powerstate)
  {
    if(g_power > g_targetPower)
    {
      g_targetCurrent = g_targetCurrent - g_stepCorr;
    }
    if(g_power < g_targetPower)
    {
      g_targetCurrent = g_targetCurrent + g_stepCorr;
    }
    SetVoltAndCurrent(g_targetVoltage, g_targetCurrent);
  }
  delay(250);
}

void serialEvent() 
{
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read();
    if(inChar == '\n') continue;
    if(inChar == '\r') continue;
    // add it to the inputString:
    g_command += inChar;
    // if the incoming character is a newline, set a flag so the main loop can
    // do something about it:
    if (inChar == ':') {
      g_commandComplete = true;
    }
  }
}
