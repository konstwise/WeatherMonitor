using System;
using System.Collections.Generic;
using System.Linq;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.OpenWeatherMapProvider.DTO
{
    public class LocationForecastResponse
    {
        public string cod { get; set; }
        public int message { get; set; }
        public int cnt { get; set; }
        public List<TimePoint> list { get; set; }

        public DailyTemperatureForecast[] MapToDailyForecast(decimal minCelsium, decimal maxCelsium)
        {
            return list.GroupBy(tp => DateTimeOffset.FromUnixTimeSeconds(tp.dt).Date)
                .Select(dailyPoints =>
                new DailyTemperatureForecast
                {
                    Date = dailyPoints.Key,
                    IsUpperLimitExceeded = dailyPoints.Max(p => p.main.temp_max) > (double) maxCelsium,
                    IsLowerLimitExceeded = dailyPoints.Min(p => p.main.temp_min) < (double) minCelsium,
                }).ToArray();
        }
    }
    
    public class TimePoint
    {
        public int dt { get; set; }
        public Main main { get; set; }
        public string dt_txt { get; set; }
    }
    
    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int sea_level { get; set; }
        public int grnd_level { get; set; }
        public int humidity { get; set; }
        public double temp_kf { get; set; }
    }
}