using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WeatherMonitor.Domain;
using WeatherMonitor.Infrastructure;

namespace WeatherMonitor.Core
{
    public class ForecastMonitor : ILongRunningTaskSource
    {
        private readonly IForecastUpdater _forecastUpdater;
        private readonly MonitoringConfig _config;

        private readonly ILogger<ForecastMonitor> _logger;

        public ForecastMonitor(IForecastUpdater forecastUpdater, MonitoringConfig config, ILogger<ForecastMonitor> logger)
        {
            _forecastUpdater = forecastUpdater ?? throw new ArgumentNullException(nameof(forecastUpdater));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task UpdateForecastPeriodically(CancellationToken stoppingToken)
        {
            _logger.LogDebug(
                $"Updating forecast checks with interval: {_config.UpdateInterval}...");
            while (!stoppingToken.IsCancellationRequested)
            {
                await _forecastUpdater.UpdateAllLocationsAsync(stoppingToken);
                await Task.Delay(_config.UpdateInterval, stoppingToken);
            }

            _logger.LogDebug("Done.");
        }

        public Task RunTaskAsync(CancellationToken stoppingToken) => UpdateForecastPeriodically(stoppingToken);
    }
}