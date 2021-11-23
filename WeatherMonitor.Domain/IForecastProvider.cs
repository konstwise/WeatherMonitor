using System.Threading.Tasks;

namespace WeatherMonitor.Domain
{
    public interface IForecastProvider
    {
        Task<DailyTemperatureRangeForecast[]> GetNextFiveDaysDailyForecast(string locationName,
            string locationCountryCode = null);
    }
}