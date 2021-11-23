using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Core
{
    public class ForecastUpdater : IForecastUpdater
    {
        private readonly IForecastChecker _forecastChecker;
        private readonly ILogger<ForecastUpdater> _logger;
        private readonly IForecastRepository _forecastRepository;
        private readonly LocationConfig[] _locations;

        public ForecastUpdater(IForecastChecker forecastChecker, ILogger<ForecastUpdater> logger, 
            MonitoringConfig config,
            IForecastRepository forecastRepository)
        {
            _forecastChecker = forecastChecker ?? throw new ArgumentNullException(nameof(forecastChecker));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _locations = config?.Locations ?? throw new ArgumentNullException(nameof(config.Locations));
            _forecastRepository = forecastRepository ?? throw new ArgumentNullException(nameof(forecastRepository));
        }

        public async Task UpdateAllLocationsAsync(CancellationToken token)
        {
            _logger.LogDebug(
                "Retrieving forecast checks for all configured locations..");

            foreach (var locationConfig in _locations)
            {
                if (token.IsCancellationRequested)
                {
                    _logger.LogInformation(
                        "Forecast update cancelled");
                    return;
                }
                
                var results = await _forecastChecker.CheckLocationForecastAsync(locationConfig);
                var location = new Location
                {
                    Name = locationConfig.Name,
                    CountryOrState = locationConfig.CountryOrState
                };
                _forecastRepository.UpdateLocationForecast(location, results);
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