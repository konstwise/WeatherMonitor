using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace WeatherMonitor.OpenWeatherMapProvider
{
    public class RetryHttpRequestSender : IRetryHttpRequestSender
    {
        private readonly OpenWeatherMapApiConfig _config;
        private readonly ILogger<RetryHttpRequestSender> _logger;

        public RetryHttpRequestSender(OpenWeatherMapApiConfig config, ILogger<RetryHttpRequestSender> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger;
        }

        public Task<HttpResponseMessage> HandleRequestWithPolicyAsync(Func<Task<HttpResponseMessage>> sendAsync)
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
                .WaitAndRetryAsync(_config.RetryPolicy.MaxRetryAttempts, 
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