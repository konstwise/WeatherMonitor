using System.Threading.Tasks;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Domain
{
    public interface IForecastProvider
    {
        Task<DailyTemperatureForecast[]> GetLocationForecastAsync(LocationConfig location);
    }
}