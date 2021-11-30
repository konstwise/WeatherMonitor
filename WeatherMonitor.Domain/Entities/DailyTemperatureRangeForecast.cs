using System;

namespace WeatherMonitor.Domain.Entities
{
    public class DailyTemperatureRangeForecast
    {
        public DateTime Date { get; set; }
        public double MaxTemperatureCelsius { get; set; }
        public double MinTemperatureCelsius { get; set; }
    }
}