using System;
using WeatherMonitor.ForecastUpdater.Tests;

namespace WeatherMonitor.OpenWeatherMapProvider
{
    public class OpenWeatherMapApiConfig
    {
        public string AppId { get; set; }
        public string BaseUrl { get; set; }
        public RetryPolicyConfig RetryPolicy { get; set; }
    }
}