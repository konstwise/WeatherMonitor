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
        private readonly IHostApplicationLifetime _applicationLifetime;

        public RecurringTaskExecutor(IExecutor task, MonitoringConfig config, ILogger<RecurringTaskExecutor> logger, IHostApplicationLifetime applicationLifetime)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger;
            _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug(
                $"Start executing with interval: {_config.UpdateInterval}");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _task.ExecuteAsync(stoppingToken);
                    await Task.Delay(_config.UpdateInterval, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, "Unrecoverable error occurred. Stopping.");
                    _applicationLifetime.StopApplication();
                }
            }            
            _logger.LogDebug("Done.");
        }
    }
}