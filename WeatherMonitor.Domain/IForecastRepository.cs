using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Domain
{
    public interface IForecastRepository
    {
        LocationForecast[] GetAllLocationForecasts();
        void Update(Location location, DailyTemperatureForecast[] forecast);
    }
}