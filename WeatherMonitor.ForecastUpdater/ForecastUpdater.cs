using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.ForecastUpdater
{
    public class ForecastUpdater : IForecastUpdater
    {
        private readonly IForecastProvider _forecastProvider;
        private readonly ILogger<ForecastUpdater> _logger;
        private readonly MonitoringConfig _config;
        private readonly IForecastRepository _forecastRepository;

        public ForecastUpdater(IForecastProvider forecastProvider, ILogger<ForecastUpdater> logger, 
            IOptions<MonitoringConfig> configOptions,
            IForecastRepository forecastRepository)
        {
            _forecastProvider = forecastProvider;
            _logger = logger;
            _config = configOptions.Value;
            _forecastRepository = forecastRepository;
        }

        public async Task UpdatePeriodicallyAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug(
                $"Updating interval: {_config.UpdateInterval}");
            while (!cancellationToken.IsCancellationRequested)
            {
                await UpdateAllLocationsAsync();

                await Task.Delay(_config.UpdateInterval, cancellationToken);
            }
        }

        private async Task UpdateAllLocationsAsync()
        {
            _logger.LogDebug(
                "Retrieving weather forecast for all configured locations..");

            foreach (var locationConfig in _config.Locations)
            {
                var forecast = await _forecastProvider.GetLocationForecastAsync(locationConfig);
                var location = new Location
                {
                    Name = locationConfig.Name,
                    CountryOrState = locationConfig.CountryOrState,
                    Latitude = locationConfig.Latitude,
                    Longitude = locationConfig.Longitude,
                };
                _forecastRepository.Update(location, forecast);
            }

            _logger.LogInformation(
                "Forecast updated for {LocationsCount} locations.", _config.Locations.Length);
        }
    }
}