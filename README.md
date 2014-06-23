IGP.Tools
=========

Projects that help to develop and maintain AMIS-RF.

Device Emulator
---------------

Device Emulator is an application that can emulate behaviour of meteorological sensors.
These sensors are using for measuring of meteo-parameters (wind, cloud, visibility, temperature and other). All of them send simple text message to a port with some time interval.

For example, ceilometer CL31 send this kind of message every 15 seconds:
```
20 00350 01200 ///// 00000000
```

Device emulator is designed to emulate a behavior of this kind of device and provide messages similar to real ones. Meteo-parameters in these messages should changes according to rules described by users.
