# StroblCap
The StroblCap is a simple Dew Cap controller to control two caps.
The Device is a simple WEMOS D1 mini WLAN device, connected to
a MQTT broker and supports eight topics:
  Astro/StroblCap/ch[1..2] to set the output power between a value of 0-100 (%)
and
  Astro/StroblCap/ch[1..2]/state to report back the power level (also 0-100).

The MQTT topics use the power value + 100, the valid range from 0-100(%) is mapped to
100-200. The payload contains only one single byte.

For a complete setup a MQTT broker is required in the network setup of the telescope.
For Win10 the mosquitto broker is a good decision. Please do not forgett to open the
firewall for the mosquitto process:

```
-----------------         ----------------         -----------------      ----------------
|               |         |              |         |               |      |              |
| StroblCap     |---------| MQTT Broker  |---------| ASCOM Driver  |------| ASCOM Client |
| WEMOS D1 mini |         | Port 1883    |         |               |      | N.I.N.A.     |
-----------------         ----------------         -----------------      ----------------
```
The ASCOM driver is compiled with Visual Studio 2019 Communitiy. It requires the M2Mqtt.Net 
dll in a signed version, otherwise ASCOM will not load the driver. The signed M2Mqtt.Net.dll 
is in the Signed3rdParty directory. It is created out of the Nuget M2Mqtt.Net package via the 
SignM2Mqtt.bat file.

'StroblCap Setup.exe' is a precompiled driver package and can be used for x64 Win10 installations.
This package installs also the EnvironmentPlot application to check for a working environment while
observing stars.

Furthermore the firmware supports two environment sensors (BMP280 only) for each channel. The small 
BMP280 is mounted inside the dew cap near the optical surface and measure the temperature and the
humidity. Out of this data the dewpoint is calculated. If the differece between the temperature and
the dewpoint is less than 5.0 °C, the heater is switched on. The heater is controlled by a simple
PI controler (in software) to set the power to a value to keep the 5.0 °C difference.

Harware setup:
```
Pin D1: SCL (I2C)
Pin D2: SDA (I2C)
Pin D5: PWM for Channel 1
Pin D6: PWM for Channel 2
```

Please be sure to use only 3.3V for powering the BMP280. The WEMOS is not 5V tolerant!

# MQTT

The complete list of MQTT topics:

```
Astro/StroblCap/ch1           The pwm value for Channel 1, payload 1 byte, 100..200 (value + 100)
Astro/StroblCap/ch2           The pwm value for Channel 2, payload 1 byte, 100..200 (value + 100)
Astro/StroblCap/ch1/OnOff     Channel 1 overall on or off, boolen value (1: false, 2: true, e.g. bool + 1)
Astro/StroblCap/ch2/OnOff     Channel 2 overall on or off, boolen value (1: false, 2: true, e.g. bool + 1)
Astro/StroblCap/ch1/auto      Indicates if the automatic PI controler should be used or not, bool, see above.
Astro/StroblCap/ch2/auto      Indicates if the automatic PI controler should be used or not, bool, see above.
```

This are the report topics. These are used as acknowledgement of the commands above.
```
Astro/StroblCap/ch1/state
Astro/StroblCap/ch2/state
Astro/StroblCap/ch1/stateOnOff
Astro/StroblCap/ch2/stateOnOff
Astro/StroblCap/ch1/stateAuto
Astro/StroblCap/ch2/stateAuto
```
This are the envoronment topic. These topics are used for the EnvironmentPlot-Application (C#, Windows):

```
Astro/StroblCap/Env/ch[1,2]/Temp      in °C
Astro/StroblCap/Env/ch[1,2]/Pressure  in hPa
Astro/StroblCap/Env/ch[1,2]/Humidity  in %
Astro/StroblCap/Env/ch[1,2]/Dewpoint  in °C
```
# EnvironmentPlot Application

The EnvironmentPlot application catch the MQTT topic for temperature, dewpoint, humidity and the power setting
for channel 1 and 2. At first startup it is only a small minimized window in the upper left corner. Place this app 
where you want to have it on the screen and the size you need. This position and size will be stored for the next startup.

![Screenshot of running StroblCap controller and N.I.N.A.](https://github.com/stroblhofwarte/StroblCap/blob/main/StroblCap_Screenshot.png)

Screenshot where the EnvironmentPlot is running in the bottom part of the screen. A BME280 sensor is connected only for channel 1, the second channel does not send any values.

