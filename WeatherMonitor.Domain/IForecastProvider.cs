using System.Threading.Tasks;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Domain
{
    public interface IForecastProvider
    {
        Task<DailyTemperatureRangeForecast[]> GetNextFiveDaysForecast(string locationName,
            string locationCountryCode = null);
    }
}