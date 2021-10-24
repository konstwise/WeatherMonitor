#!/bin/bash

#echo "Running latest docker image in Development mode"
#sudo docker run -d --env ASPNETCORE_ENVIRONMENT=Development cloudatortask/weather-monitor:latest

echo "Running latest docker image in Production mode"
sudo docker run -d cloudatortask/weather-monitor:latest
