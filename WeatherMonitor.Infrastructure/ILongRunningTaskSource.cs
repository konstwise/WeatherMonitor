using System.Threading;
using System.Threading.Tasks;

namespace WeatherMonitor.Infrastructure
{
    public interface ILongRunningTaskSource
    {
        Task RunTaskAsync(CancellationToken stoppingToken);
    }
}