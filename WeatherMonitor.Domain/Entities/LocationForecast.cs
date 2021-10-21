namespace WeatherMonitor.Domain.Entities
{
    public record LocationForecast(Location Location, DailyTemperatureForecast[] DailyForecast);
}