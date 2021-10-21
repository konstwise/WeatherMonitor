using System;

namespace WeatherMonitor.Domain.Entities
{
    public class DailyTemperatureForecast
    {
        public DateTime Date { get; set; }

        public bool IsUpperLimitExceeded { get; set; }

        public bool IsLowerLimitExceeded { get; set; }
    }
}