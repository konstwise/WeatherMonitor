using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;
using WeatherMonitor.ForecastUpdater.Tests;
using WeatherMonitor.OpenWeatherMapProvider.DTO;
using WeatherMonitor.Services;

namespace WeatherMonitor.OpenWeatherMapProvider
{
    public class OpenWeatherMapForecastProvider: IForecastProvider
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapApiConfig _apiConfig;
        private readonly ILogger<OpenWeatherMapForecastProvider> _logger;
        private readonly IRetryHttpRequestHandler _retryHttpRequestSender;

        public OpenWeatherMapForecastProvider(HttpClient httpClient, OpenWeatherMapApiConfig apiConfig, ILogger<OpenWeatherMapForecastProvider> logger, IRetryHttpRequestHandler retryHttpRequestSender)
        {
            _httpClient = httpClient;
            _apiConfig = apiConfig;
            _logger = logger;
            _retryHttpRequestSender = retryHttpRequestSender;
        }

        public async Task<DailyTemperatureForecast[]> GetLocationForecastAsync(LocationConfig location)
        {
            string countryOrState = location.CountryOrState is null ? "" : $",{location.CountryOrState}";
            string place = $"{location.Name}{countryOrState}";
            var res = await _retryHttpRequestSender.HandleWithPolicyAsync(() =>
                _httpClient.GetAsync($"forecast?q={place}&appid={_apiConfig.AppId}&units=metric"));

            res.EnsureSuccessStatusCode();
            var dto = (await res.Content.ReadFromJsonAsync<LocationForecastResponse>())!;
            return dto.MapToDailyForecast(minCelsium: location.Limits.LowerCelsium, maxCelsium: location.Limits.UpperCelsium);
        }
    }
}
