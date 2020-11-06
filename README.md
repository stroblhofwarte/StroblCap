# StroblCap
The StroblCap is a simple Dew Cap controller to control four caps.
The Device is a aimple WEMOS D1 mini WLAN device, connected to
a MQTT broker and supports eight topics:
  Astro/StroblCap/ch[1..4] to set the output power between a value of 0-100 (%)
and
  Astro/StroblCap/ch[1..4]/state to report back the power level (also 0-100).

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

The hardware for this project based on a WEMOS D1 mini and controls the dew caps with a single 
MOSFET per channel via pwm. 
Furthermore the firmware supports two environment sensors (BMP280 only) for each channel. The small 
BMP280 is mounted inside the dew cap near the optical surface and measure the temperature and the
humidity. Out of this data the dewpoint is calculated. If the differece between the temperature and
the dewpoint is less than 5.0 °C, the heater is switched on. The heater is controlled by a simple
PI controler (in software) to set the power to a value to keep the 5.0 °C difference.
