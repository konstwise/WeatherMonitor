using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WeatherMonitor.Infrastructure
{
    public class BackgroundWorker : BackgroundService
    {
        private readonly ILongRunningTaskSource _taskSource;
        private readonly ILogger<BackgroundWorker> _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public BackgroundWorker(ILongRunningTaskSource taskSource, ILogger<BackgroundWorker> logger, IHostApplicationLifetime applicationLifetime)
        {
            _taskSource = taskSource ?? throw new ArgumentNullException(nameof(taskSource));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug(
                $"Launching task from the source...");
            try
            {
                await _taskSource.RunTaskAsync(stoppingToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Task running cancelled.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Unhandled exception occurred. Stopping application.");
                _applicationLifetime.StopApplication();
            }

            _logger.LogDebug("Long running task done.");
        }
    }
}