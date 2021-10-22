using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WeatherMonitor.Services
{
    public class LongRunningTaskExecutor : BackgroundService
    {
        private readonly Func<CancellationToken, Task> _launcher;
        private readonly ILogger<LongRunningTaskExecutor> _logger;

        public LongRunningTaskExecutor(Func<CancellationToken, Task> launcher, ILogger<LongRunningTaskExecutor> logger)
        {
            _launcher = launcher;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug(
                $"Launching task...");
            await _launcher(stoppingToken);
            _logger.LogDebug("Task done.");
        }
    }
}