using System.Collections.Concurrent;
using System.Linq;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Domain
{
    public class ForecastRepository : IForecastRepository
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