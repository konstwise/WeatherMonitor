using System.Threading;
using System.Threading.Tasks;

namespace WeatherMonitor.Domain
{
    public interface IForecastUpdater
    {
        Task UpdateAllLocationsAsync(CancellationToken cancellationToken);
    }
}