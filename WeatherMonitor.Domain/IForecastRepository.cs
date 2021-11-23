using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Domain
{
    public interface IForecastRepository
    {
        LocationForecastCheckResults[] GetAllLocationCheckResults();
        void UpdateLocationForecast(Location location, DailyForecastCheckResult[] forecast);
    }
}