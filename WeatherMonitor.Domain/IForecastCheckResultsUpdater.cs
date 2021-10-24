using System.Threading;
using System.Threading.Tasks;

namespace WeatherMonitor.Domain
{
    public interface IForecastCheckResultsUpdater
    {
        Task UpdateAllLocationsAsync(CancellationToken cancellationToken);
    }
}