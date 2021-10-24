using System.Threading.Tasks;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Domain
{
    /// <summary>
    /// Provides forecast checks for the next 5 days according to the location and
    /// temperature limits specified
    /// </summary>
    public interface IForecastChecker
    {
        /// <summary>
        /// Checks current forecast for next 5 days for the given location and returns indication
        /// whether the configured limits exceeded per day
        /// </summary>
        /// <param name="location">Location name and temperature range to be checked</param>
        /// <returns>Forecast check results for <paramref name="location"/></returns>
        Task<DailyForecastCheckResult[]> CheckLocationForecastAsync(LocationConfig location);
    }
}