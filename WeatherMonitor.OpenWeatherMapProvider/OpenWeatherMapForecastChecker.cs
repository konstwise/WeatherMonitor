using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;
using WeatherMonitor.OpenWeatherMapProvider.DTO;
using WeatherMonitor.Infrastructure;

namespace WeatherMonitor.OpenWeatherMapProvider
{
    /// <summary>
    /// Implements <c>IForecastChecker</c> for Open Weather public API https://openweathermap.org/api
    /// </summary>
    public class OpenWeatherMapForecastChecker: IForecastChecker
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapApiConfig _apiConfig;
        private readonly ILogger<OpenWeatherMapForecastChecker> _logger;
        private readonly IRetryHttpRequestHandler _retryHttpRequestSender;

        public OpenWeatherMapForecastChecker(HttpClient httpClient, OpenWeatherMapApiConfig apiConfig, ILogger<OpenWeatherMapForecastChecker> logger, IRetryHttpRequestHandler retryHttpRequestSender)
        {
            _httpClient = httpClient;
            _apiConfig = apiConfig;
            _logger = logger;
            _retryHttpRequestSender = retryHttpRequestSender;
        }
        
        /// <inheritdoc cref="IForecastChecker"/>>
        public async Task<DailyForecastCheckResult[]> CheckLocationForecastAsync(LocationConfig location)
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
