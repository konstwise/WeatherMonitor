using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Core
{
    public class ForecastUpdater : IForecastUpdater, IExecutor
    {
        private readonly IForecastProvider _forecastProvider;
        private readonly ILogger<ForecastUpdater> _logger;
        private readonly IForecastRepository _forecastRepository;
        private readonly LocationConfig[] _locations;

        public ForecastUpdater(IForecastProvider forecastProvider, ILogger<ForecastUpdater> logger, 
            MonitoringConfig config,
            IForecastRepository forecastRepository)
        {
            _forecastProvider = forecastProvider;
            _logger = logger;
            _locations = config?.Locations ?? throw new ArgumentNullException(nameof(config.Locations));
            _forecastRepository = forecastRepository;
        }

        public async Task UpdateAllLocationsAsync(CancellationToken token)
        {
            _logger.LogDebug(
                "Retrieving weather forecast for all configured locations..");

            foreach (var locationConfig in _locations)
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
                "Forecast updated for {LocationsCount} locations.", _locations.Length);
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await UpdateAllLocationsAsync(stoppingToken);
        }
        
    }
}