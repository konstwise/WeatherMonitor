namespace WeatherMonitor.Domain.Entities
{
    public record Location
    {
        public string Name { get; init; }
        public string CountryOrState { get; init; }
    }
}