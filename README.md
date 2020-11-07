
# StroblCap
The StroblCap is a simple Dew Cap controller to control two caps.
The Device is a simple WEMOS D1 mini WLAN device. The communication is done 
via the COM port.
This device behaves like a switch device. Each output channel could be set to
a power output between 0 and 100%. Each channel can be equipped with a BME280
environment sensor for a automatic dew control. Each channel can be switched on and 
off without changing the power settings. Each channel can be switched to the automatic
dew control or not.

The ASCOM driver is compiled with Visual Studio 2019 Communitiy. The ASCOM driver does not
block the COM device, the COM port is in use only when new switch settings are send
from the ASCOM driver to the device. This allows to run a environment plot application (EnvironmentPlot.exe) 
in parallel to observe the measurement values of the BME280 sensors. Furthermore the power 
settings via ASCOM driver or from the automatic dew control can be observed.

'StroblCap Setup.exe' is a precompiled driver package and can be used for x64 Win10 installations.
This package installs also the EnvironmentPlot application to check for a working environment while
observing stars.

# Firmware

The firmware supports two environment sensors (BMP280 only) for each channel. The small 
BMP280 is mounted inside the dew cap near the optical surface and measure the temperature and the
humidity. Out of this data the dewpoint is calculated. If the differece between the temperature and
the dewpoint is less than 5.0 °C, the heater is switched on. The heater is controlled by a simple
PI controler (in software) to set the power to a value to keep the 5.0 °C difference.
The firmware is written in the arduino development environment.

Harware setup:
```
Pin D1: SCL (I2C)
Pin D2: SDA (I2C)
Pin D5: PWM for Channel 1
Pin D6: PWM for Channel 2
```

Please be sure to use only 3.3V for powering the BMP280. The WEMOS is not 5V tolerant!

The firmware supports following commands via serial communication line (without CR, LineFeed etc.). 
The general command format is "cmd:", the response to a command is always postfixed with a #.


```
  "ID:"            Sent back the device type id with "STROBLCAP#"
  "S1nnn:"         Set power for channel 1 to nnn, where nnn is 000 - 100 [%]
  "S2nnn:"         Set power for channel 2 to nnn, return "1#" when set, otherwise "0#"
  "E1:"            Enable channel 1. "1#" if ok
  "E2:"            Enable channel 2. "1#" if ok
  "D1:"            Disable channel 1
  "D2:"            Disable channel 2
  "A1:"            Set channel 1 to automatic mode. "1#" if ok, "0#" when no sensor was found for this channel
  "A2:"            Set channel 2 to automatic mode. "1#" if ok, "0#" when no sensor for this channel
  "M1:"            Set channel 1 to manual mode
  "M2:"            Set channel 2 to manual mode
  "G1:"            Get sensor data channel 1: "t;h;d;p#", where
                             t: Temperature * 100 [°C],
                             h: Humidity * 100 [%],
                             d: Dewpoint * 100 [°C],
                             p: Pressure * 10 [hPa]
  "G2:"            Get sensor data channel 2: "t;h;d;p#", where
                             t: Temperature * 100 [°C],
                             h: Humidity * 100 [%],
                             d: Dewpoint * 100 [°C],
                             p: Pressure * 10 [hPa]
  "P1:"            Get power value channel 1, "nnn#"
  "P2:"            Get power value channel 2, "nnn#"
```

# EnvironmentPlot Application

The EnvironmentPlot application catch via the COM port in parallel to the ASCOM driver the values for temperature, 
dewpoint, humidity and the power setting for channel 1 and 2. At first startup it is only a small minimized window in 
the upper left corner. Place this app where you want to have it on the screen and the size you need. 
This position and size will be stored for the next startup. Do not forgett to set the COM port of the StroblCap device!

![Screenshot of running StroblCap controller and N.I.N.A.](https://github.com/stroblhofwarte/StroblCap/blob/main/StroblCap_Screenshot.png)

Screenshot where the EnvironmentPlot is running on the bottom part of the screen. A BME280 sensor is connected only for channel 1, the second channel send always zero for all values because there is no sensor. The app will not show this values! The N.I.N.A. application is running in parallel to control the settings 
of the StroblCap device.

