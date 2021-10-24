#!/bin/bash
echo "Building from Dockerfile..."
sudo docker build . -t cloudatortask/weather-monitor:latest 
