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
        public TemperatureLimits Limits { get; init; }

    }

    public record TemperatureLimits
    {
        public decimal LowerCelsius { get; set; }
        public decimal UpperCelsius { get; set; }
    }
}