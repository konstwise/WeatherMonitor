using System;

namespace WeatherMonitor.Domain
{
    public class DailyTemperatureRangeForecast
    {
        public DateTime Date { get; set; }
        public double MaxTemperatureCelsium { get; set; }
        public double MinTemperatureCelsium { get; set; }
    }
}