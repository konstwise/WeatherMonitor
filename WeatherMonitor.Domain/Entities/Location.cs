namespace WeatherMonitor.Domain.Entities
{
    public record Location
    {
        public string Name { get; init; }
        public string CountryOrState { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
    }
}