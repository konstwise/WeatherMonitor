using System.Threading;
using System.Threading.Tasks;

namespace WeatherMonitor.Domain
{
    public interface IExecutor
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}