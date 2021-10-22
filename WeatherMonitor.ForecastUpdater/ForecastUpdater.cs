using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.ForecastUpdater
{
    public class ForecastUpdater : IForecastUpdater, IExecutor
    {
        private readonly IForecastProvider _forecastProvider;
        private readonly ILogger<ForecastUpdater> _logger;
        private readonly MonitoringConfig _config;
        private readonly IForecastRepository _forecastRepository;

        public ForecastUpdater(IForecastProvider forecastProvider, ILogger<ForecastUpdater> logger, 
            MonitoringConfig config,
            IForecastRepository forecastRepository)
        {
            _forecastProvider = forecastProvider;
            _logger = logger;
            _config = config;
            _forecastRepository = forecastRepository;
        }

        public async Task UpdateAllLocationsAsync(CancellationToken token)
        {
            _logger.LogDebug(
                "Retrieving weather forecast for all configured locations..");

            foreach (var locationConfig in _config.Locations)
            {
                if (token.IsCancellationRequested)
                {
                    _logger.LogInformation(
                        "Forecast update cancelled");
                    return;
                }
                
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

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await UpdateAllLocationsAsync(stoppingToken);
        }
    }
}