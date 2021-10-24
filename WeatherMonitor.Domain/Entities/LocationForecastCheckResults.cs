namespace WeatherMonitor.Domain.Entities
{
    public record LocationForecastCheckResults(Location Location, DailyForecastCheckResult[] DailyForecast);
}