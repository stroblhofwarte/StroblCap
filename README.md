# StroblCap
The StroblCap is a simple Dew Cap controller to control four caps.
The Device is a aimple WEMOS D1 mini WLAN device, connected to
a MQTT broker and supports eight topics:
  Astro/StroblCap/ch[1..4] to set the output power between a value of 0-100 (%)
and
  Astro/StroblCap/ch[1..4]/state to report back the power level (also 0-100).
 
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

