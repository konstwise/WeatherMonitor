using System;
using System.Linq;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Core
{
    public class ForecastRepository : IForecastRepository
    {
        private readonly IKeyValueStore<Location, DailyForecastCheckResult[]> _keyValueStore;

        public ForecastRepository(IKeyValueStore<Location, DailyForecastCheckResult[]> keyValueStore)
        {
            _keyValueStore = keyValueStore ?? throw new ArgumentNullException(nameof(keyValueStore));
        }

        public LocationForecastCheckResults[] GetAllLocationCheckResults()
        {
            var locations = _keyValueStore.GetAllKeys();

            return locations.Select(location =>
                new LocationForecastCheckResults(location, _keyValueStore.GetValue(location)))
                .ToArray();
        }

        public void UpdateLocationForecast(Location location, DailyForecastCheckResult[] forecast)
        {
            _keyValueStore.UpdateValue(location, forecast);
        }
    }
}