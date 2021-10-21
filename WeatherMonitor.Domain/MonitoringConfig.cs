using System;

namespace WeatherMonitor.Domain
{
    public class MonitoringConfig
    {
        public TimeSpan UpdateInterval { get; set; }
        public LocationConfig[] Locations { get; set; }
    }

    public record LocationConfig
    {
        public string Name { get; init; }
        public string CountryOrState { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
        public TemperatureLimits Limits { get; init; }

    }

    public record TemperatureLimits
    {
        public decimal LowerCelsium { get; set; }
        public decimal UpperCelsium { get; set; }
    }
}