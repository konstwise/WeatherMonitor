using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WeatherMonitor.OpenWeatherMapProvider.DTO
{
    public class LocationForecastResponse
    {
        [JsonPropertyName("list")]
        public List<TimePoint> FiveDaysThreeHoursForecast { get; set; }
    }
    
    public class TimePoint
    {
        [JsonPropertyName("dt")]
        public int TimestampUnix { get; set; }
        [JsonPropertyName("main")]
        public DailyMinMax DailyMinMax { get; set; }
    }
    
    public class DailyMinMax
    {
        [JsonPropertyName("temp_min")]
        public double MinimumTemperature { get; set; }
        [JsonPropertyName("temp_max")]
        public double MaximumTemperature { get; set; }
    }
}