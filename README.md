### Weather Monitor API
The API serves the latest data containing weather forecast checks for the next 5 days for the predefined locations.
Each check data itself contains indications whether the configured temperature limits are going to be exceeded or not for the particular date.

### Usage
The single endpoint can be requested as follows:

#### 
    GET /WeatherForecast 

The response contains a collection of data with the following shape:

````json
[LocationForecastCheckResults{
location	Location{
name	string
nullable: true
countryOrState	string
nullable: true
}
dailyForecast	[
nullable: true
DailyForecastCheckResult{
date	string($date-time)
isUpperLimitExceeded	boolean
isLowerLimitExceeded	boolean
}]
}]
````
For example:
````json
[
  {
    "location": {
      "name": "Copenhagen",
      "countryOrState": "DK"
    },
    "dailyForecast": [
      {
        "date": "2021-10-24T00:00:00",
        "isUpperLimitExceeded": false,
        "isLowerLimitExceeded": false
      },
      {
        "date": "2021-10-25T00:00:00",
        "isUpperLimitExceeded": false,
        "isLowerLimitExceeded": false
      },
      {
        "date": "2021-10-26T00:00:00",
        "isUpperLimitExceeded": false,
        "isLowerLimitExceeded": false
      },
      {
        "date": "2021-10-27T00:00:00",
        "isUpperLimitExceeded": true,
        "isLowerLimitExceeded": false
      },
      {
        "date": "2021-10-28T00:00:00",
        "isUpperLimitExceeded": true,
        "isLowerLimitExceeded": false
      },
      {
        "date": "2021-10-29T00:00:00",
        "isUpperLimitExceeded": false,
        "isLowerLimitExceeded": false
      }
    ]
  },
]
````

### Build Instructions

There are a few bash scripts for that purpose (checked in Ubuntu but should be compatible with Mac OSX).

_Please make them executable by running <chmod u+x [script].sh>_ 

**[Build](build.sh)** docker image on Linux from WeatherMonitor directory:
####
    ./build.sh

**Test** by running:
####
    ./test.sh

**Run** as docker container:
- in production mode:
####
    ./run.sh
- in development mode with Swagger UI enabled:
####
    see comments in run.sh

**Explore** in browser Swagger UI or/and sending requests to API:
- open Swagger UI:
####
    ./open-swagger.sh

- send request by curl:
####
    curl -X GET "http://$INSTANCE_IP/WeatherForecast" -H  "accept: application/json"
