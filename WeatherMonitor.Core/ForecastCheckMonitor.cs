using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WeatherMonitor.Domain;
using WeatherMonitor.Infrastructure;

namespace WeatherMonitor.Core
{
    public class ForecastCheckMonitor : ILongRunningTaskSource
    {
        private readonly IForecastCheckResultsUpdater _forecastCheckResultsUpdater;
        private readonly MonitoringConfig _config;

        private readonly ILogger<ForecastCheckMonitor> _logger;

        public ForecastCheckMonitor(IForecastCheckResultsUpdater forecastCheckResultsUpdater, MonitoringConfig config, ILogger<ForecastCheckMonitor> logger)
        {
            _forecastCheckResultsUpdater = forecastCheckResultsUpdater ?? throw new ArgumentNullException(nameof(forecastCheckResultsUpdater));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task UpdateForecastCheckResultsPeriodicallyAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug(
                $"Updating forecast checks with interval: {_config.UpdateInterval}...");
            while (!stoppingToken.IsCancellationRequested)
            {
                await _forecastCheckResultsUpdater.UpdateAllLocationsAsync(stoppingToken);
                await Task.Delay(_config.UpdateInterval, stoppingToken);
            }

            _logger.LogDebug("Done.");
        }

        public Task RunTaskAsync(CancellationToken stoppingToken) => UpdateForecastCheckResultsPeriodicallyAsync(stoppingToken);
    }
}