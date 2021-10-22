Build docker image on Linux from WeatherMonitor directory:
####
    sudo docker build . -t cloudatortask/weather-monitor

Run:
- in production mode:
####
    sudo docker run -d cloudatortask/weather-monitor
- in development mode with Swagger UI enabled:
####
    sudo docker run -d --env ASPNETCORE_ENVIRONMENT=Development cloudatortask/weather-monitor

To explore Swagger UI or/and sending requests to API:
- find out container host IP:
####
    export INSTANCE_ID=$(sudo docker  ps  -qf "ancestor=cloudatortask/weather-monitor")
    export INSTANCE_IP=$(sudo docker inspect --format='{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' 34b5db250dab)
    echo $INSTANCE_IP

- explore Swagger UI:
####
    xdg-open http://$INSTANCE_IP/swagger/index.html

- send request by curl:
####
    curl -X GET "http://$INSTANCE_IP/WeatherForecast" -H  "accept: text/json"
