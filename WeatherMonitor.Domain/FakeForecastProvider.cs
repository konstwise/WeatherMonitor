using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.ForecastProviders
{
    public class FakeForecastProvider : IForecastProvider
    {
        private readonly Random _rng = new Random();

        public async Task<DailyTemperatureForecast[]> GetLocationForecastAsync(LocationConfig location)
        {
            return await Task.FromResult(
                Enumerable.Range(1, 5)
                    .Select(day => (day, temp: _rng.Next(-20, 55)))
                    .Select(dt => new DailyTemperatureForecast
                    {
                        Date = DateTime.Now.AddDays(dt.day).Date,
                        IsLowerLimitExceeded = dt.temp < location.Limits.LowerCelsium,
                        IsUpperLimitExceeded = dt.temp > location.Limits.UpperCelsium
                    }).ToArray());
        }
    }
}