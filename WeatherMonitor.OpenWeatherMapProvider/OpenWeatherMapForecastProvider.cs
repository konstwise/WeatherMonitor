using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;
using WeatherMonitor.OpenWeatherMapProvider.DTO;

namespace WeatherMonitor.OpenWeatherMapProvider
{
    public class OpenWeatherMapForecastProvider: IForecastProvider
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapApiConfig _apiConfig;

        public OpenWeatherMapForecastProvider(HttpClient httpClient, IOptions<OpenWeatherMapApiConfig> apiConfigOptions)
        {
            _httpClient = httpClient;
            _apiConfig = apiConfigOptions.Value;
        }

        public async Task<DailyTemperatureForecast[]> GetLocationForecastAsync(LocationConfig location)
        {
            string countryOrState = location.CountryOrState is null ? "" : $",{location.CountryOrState}";
            string place = $"{location.Name}{countryOrState}";
            var res = await _httpClient.GetAsync($"forecast?q={place}&appid={_apiConfig.AppId}&units=metric");

            res.EnsureSuccessStatusCode();
            var dto = await res.Content.ReadFromJsonAsync<LocationForecastResponse>();
            return dto.MapToDailyForecast(minCelsium: location.Limits.LowerCelsium, maxCelsium: location.Limits.UpperCelsium);
        }
    }
    
    
  
}
