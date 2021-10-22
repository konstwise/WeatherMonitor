using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherMonitor.Domain;

namespace WeatherMonitor.Services
{
    public class RecurringTaskExecutor : BackgroundService
    {
        private readonly IExecutor _task;
        private readonly MonitoringConfig _config;

        private readonly ILogger<RecurringTaskExecutor> _logger;

        public RecurringTaskExecutor(IExecutor task, MonitoringConfig config, ILogger<RecurringTaskExecutor> logger)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug(
                $"Start executing with interval: {_config.UpdateInterval}");
            while (!stoppingToken.IsCancellationRequested)
            {
                await _task.ExecuteAsync(stoppingToken);
                await Task.Delay(_config.UpdateInterval, stoppingToken);
            }            
            _logger.LogDebug("Done.");
        }
    }
}