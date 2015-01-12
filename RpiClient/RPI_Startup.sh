#!/bin/bash
# /etc/init.d/RPI_Startup.sh

### BEGIN INIT INFO
# Provides:          RPI_Startup
# Required-Start:    $all
# Required-Stop:     $remote_fs $syslog
# Default-Start:     2 3 4 5
# Default-Stop:      0 1 6
# Short-Description: RPIRemoteStart
# Description:       This service is used to fire up my remote start 3G connection and client program
### END INIT INFO

echo "I am trying to run this shit mang."
sudo /usr/bin/wvdial > dial_log.txt 2> dial_err.txt & 
echo "I started wvdial" 
sleep 17 
echo "Back from sleep" 
sudo /usr/bin/python /home/pi/TestClient.py
echo "I started your python client as well"
