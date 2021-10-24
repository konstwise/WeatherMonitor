There are 3 scripts for bash/Linux:
#####
     Please make them executable by running <chmod u+x [script].sh>

Build docker image on Linux from WeatherMonitor directory:
####
    ./build.sh

Test by running:
####
    ./test.sh

Run:
- in production mode:
####
    ./run.sh
- in development mode with Swagger UI enabled:
####
    see comments in run.sh

To explore Swagger UI or/and sending requests to API:
- open Swagger UI:
####
    ./open-swagger.sh

- send request by curl:
####
    curl -X GET "http://$INSTANCE_IP/WeatherForecast" -H  "accept: text/json"
