using System.Collections.Concurrent;
using System.Linq;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;
using WeatherMonitor.ForecastUpdater.Tests;

namespace WeatherMonitor.InMemoryForecastRepository
{
    public class InMemoryForecastRepository : IForecastRepository
    {
        private readonly ConcurrentDictionary<Location, DailyTemperatureForecast[]> _internalStore =
            new();
        public LocationForecast[] GetAllLocationForecasts()
        {
            return _internalStore.Select(kv => 
                new LocationForecast(kv.Key, kv.Value)).ToArray();
        }

        public void Update(Location location, DailyTemperatureForecast[] forecast)
        {
            _internalStore[location] = forecast;
        }
    }
}