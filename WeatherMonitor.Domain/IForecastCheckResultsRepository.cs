using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Domain
{
    public interface IForecastCheckResultsRepository
    {
        LocationForecastCheckResults[] GetAllLocationCheckResults();
        void Update(Location location, DailyForecastCheckResult[] forecast);
    }
}