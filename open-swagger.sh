#!/bin/bash

export INSTANCE_ID=$(sudo docker  ps  -qf "ancestor=cloudatortask/weather-monitor")
export INSTANCE_IP=$(sudo docker inspect --format='{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' $INSTANCE_ID)
echo $INSTANCE_IP

xdg-open http://$INSTANCE_IP/swagger/index.html
