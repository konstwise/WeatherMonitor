using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WeatherMonitor.Domain;
using WeatherMonitor.OpenWeatherMapProvider.DTO;
using WeatherMonitor.Infrastructure;

namespace WeatherMonitor.OpenWeatherMapProvider
{
    /// <summary>
    /// Implements <c>IForecastChecker</c> for Open Weather public API https://openweathermap.org/api
    /// </summary>
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
        
        /// <inheritdoc cref="IForecastChecker"/>>
        public async Task<DailyTemperatureRangeForecast[]> GetNextFiveDaysDailyForecast(string locationName, string locationCountryCode = null)
        {
            var countryOrState = locationCountryCode is null ? "" : $",{locationCountryCode}";
            var place = $"{locationName}{countryOrState}";
            var res = await _retryHttpRequestSender.HandleWithPolicyAsync(() =>
                _httpClient.GetAsync($"forecast?q={place}&appid={_apiConfig.AppId}&units=metric"));

            res.EnsureSuccessStatusCode();
            var response = (await res.Content.ReadFromJsonAsync<LocationForecastResponse>())!;
            
            return response.list.GroupBy(tp => DateTimeOffset.FromUnixTimeSeconds(tp.dt).Date)
                .Select(dailyPoints =>
                    new DailyTemperatureRangeForecast
                    {
                        Date = dailyPoints.Key,
                        MaxTemperatureCelsium = dailyPoints.Max(p => p.main.temp_max),
                        MinTemperatureCelsium = dailyPoints.Min(p => p.main.temp_min)
                    }).ToArray();
        }
    }
}
