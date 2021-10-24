using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Core
{
    public class ForecastCheckResultsUpdater : IForecastCheckResultsUpdater
    {
        private readonly IForecastChecker _forecastChecker;
        private readonly ILogger<ForecastCheckResultsUpdater> _logger;
        private readonly IForecastCheckResultsRepository _forecastCheckResultsRepository;
        private readonly LocationConfig[] _locations;

        public ForecastCheckResultsUpdater(IForecastChecker forecastChecker, ILogger<ForecastCheckResultsUpdater> logger, 
            MonitoringConfig config,
            IForecastCheckResultsRepository forecastCheckResultsRepository)
        {
            _forecastChecker = forecastChecker ?? throw new ArgumentNullException(nameof(forecastChecker));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _locations = config?.Locations ?? throw new ArgumentNullException(nameof(config.Locations));
            _forecastCheckResultsRepository = forecastCheckResultsRepository ?? throw new ArgumentNullException(nameof(forecastCheckResultsRepository));
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
                _forecastCheckResultsRepository.Update(location, results);
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