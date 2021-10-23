using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;
using WeatherMonitor.OpenWeatherMapProvider.DTO;

namespace WeatherMonitor.OpenWeatherMapProvider
{
    public class OpenWeatherMapForecastProvider: IForecastProvider
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapApiConfig _apiConfig;
        private readonly ILogger<OpenWeatherMapForecastProvider> _logger;

        public OpenWeatherMapForecastProvider(HttpClient httpClient, OpenWeatherMapApiConfig apiConfig, ILogger<OpenWeatherMapForecastProvider> logger)
        {
            _httpClient = httpClient;
            _apiConfig = apiConfig;
            _logger = logger;
        }

        public async Task<DailyTemperatureForecast[]> GetLocationForecastAsync(LocationConfig location)
        {
            string countryOrState = location.CountryOrState is null ? "" : $",{location.CountryOrState}";
            string place = $"{location.Name}{countryOrState}";
            var res = await HandleRequestWithPolicyAsync(() =>
                _httpClient.GetAsync($"forecast?q={place}&appid={_apiConfig.AppId}&units=metric"));

            res.EnsureSuccessStatusCode();
            var dto = (await res.Content.ReadFromJsonAsync<LocationForecastResponse>())!;
            return dto.MapToDailyForecast(minCelsium: location.Limits.LowerCelsium, maxCelsium: location.Limits.UpperCelsium);
        }
        
        private Task<HttpResponseMessage> HandleRequestWithPolicyAsync(Func<Task<HttpResponseMessage>> sendAsync)
        {
            var fallbackRetryPolicy = Policy
                .Handle<Exception>(x =>
                {
                    _logger.LogWarning($"Forever retrying after: {x.Message}");
                    return true;
                })
                .RetryForeverAsync(onRetry: (ex, retryAttempt, _) =>
                {
                    _logger.LogWarning($"Making retry {retryAttempt}");
                });
            
            var backoffRetry = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<Exception>(ex =>
                {
                    var match = ex.Message.Contains("The request was canceled due to the configured HttpClient.Timeout", StringComparison.InvariantCultureIgnoreCase);
                    if (match)
                    {
                        _logger.LogWarning($"Http request timeout: {ex.Message}");
                    }
                    else
                    {
                        match = ex.InnerException != null &&
                                    ex.InnerException.Message.Contains("Unable to read data from the transport connection", StringComparison.InvariantCultureIgnoreCase);
                        if (match) _logger.LogWarning($"IO exception occurred: {ex.InnerException.Message}");
                    }
                    return match;
                })
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(_apiConfig.RetryPolicy.MaxRetryAttempts, 
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (outcome, timespan, retryAttempt, context) =>
                        {
                            _logger.LogWarning($"Delaying for {timespan.TotalMilliseconds}ms, then making retry {retryAttempt}.");
                        });
            return fallbackRetryPolicy
                .WrapAsync(backoffRetry)
                .ExecuteAsync(() => sendAsync());
        }
    }
}
