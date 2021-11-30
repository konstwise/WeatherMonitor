using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;
using WeatherMonitor.OpenWeatherMapProvider.DTO;
using WeatherMonitor.Infrastructure;

namespace WeatherMonitor.OpenWeatherMapProvider
{
    /// <summary>
    /// Implements <c>IForecastProvider</c> for Open Weather public API https://openweathermap.org/api
    /// </summary>
    public class OpenWeatherMapForecastProvider: IForecastProvider
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapApiConfig _apiConfig;
        private readonly IRetryHttpRequestHandler _retryHttpRequestSender;

        public OpenWeatherMapForecastProvider(HttpClient httpClient, OpenWeatherMapApiConfig apiConfig, IRetryHttpRequestHandler retryHttpRequestSender)
        {
            _httpClient = httpClient;
            _apiConfig = apiConfig;
            _retryHttpRequestSender = retryHttpRequestSender;
        }
        
        /// <inheritdoc cref="IForecastProvider"/>>
        public async Task<DailyTemperatureRangeForecast[]> GetNextFiveDaysForecast(string locationName, string locationCountryCode = null)
        {
            var countryOrState = locationCountryCode is null ? "" : $",{locationCountryCode}";
            var place = $"{locationName}{countryOrState}";
            var res = await _retryHttpRequestSender.HandleWithPolicyAsync(() =>
                _httpClient.GetAsync($"forecast?q={place}&appid={_apiConfig.AppId}&units=metric"));

            res.EnsureSuccessStatusCode();
            var response = (await res.Content.ReadFromJsonAsync<LocationForecastResponse>())!;
            
            return response?.FiveDaysThreeHoursForecast.GroupBy(tp => DateTimeOffset.FromUnixTimeSeconds(tp.TimestampUnix).Date)
                .Select(dailyPoints =>
                    new DailyTemperatureRangeForecast
                    {
                        Date = dailyPoints.Key,
                        MaxTemperatureCelsius = dailyPoints.Max(p => p.DailyMinMax.MaximumTemperature),
                        MinTemperatureCelsius = dailyPoints.Min(p => p.DailyMinMax.MinimumTemperature)
                    }).ToArray();
        }
    }
}
