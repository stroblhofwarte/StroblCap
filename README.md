
# StroblCap
The StroblCap is a simple Dew Cap controller for controling two caps.
The Device is a simple WEMOS D1 mini WLAN device. The communication is done 
via the COM port.
This device behaves like a switch device. Each output channel could be set to
a power output between 0 and 100%. Each channel can be equipped with a BME280
environment sensor for a automatic dew control. Each channel can be switched on and 
off without changing the power settings. Each channel can be switched to the automatic
dew control or not.

The ASCOM driver is compiled with Visual Studio 2019 Communitiy. 

'StroblCap Setup.exe' is a precompiled driver package and can be used for x64 Win10 installations.

(18.01.2021) Bug: The automatic dew control is not running yet.

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

The EnvironmentPlot application is a ASCOM client for the StroblCap device. This application can be used when your 
astronomy software does not support ASCOM switch devices like SGP < 4.0.

![Screenshot, EnvronmentPlot](https://github.com/stroblhofwarte/StroblCap/blob/main/
EnvironmentPlot.png )

