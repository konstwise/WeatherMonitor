using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherMonitor.Domain;

namespace WeatherMonitor.OpenWeatherMapProvider
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenWeatherMapProvider(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IForecastProvider, OpenWeatherMapForecastProvider>();
            OpenWeatherMapApiConfig config = new();
            configuration.GetSection(
                    nameof(OpenWeatherMapApiConfig))
                .Bind(config);

            services.AddSingleton(config);
            services.AddSingleton<IRetryHttpRequestSender, RetryHttpRequestSender>();
            services.AddHttpClient<IForecastProvider, OpenWeatherMapForecastProvider>(c =>
            {
                c.BaseAddress = new Uri(config.BaseUrl);
            });
            
            
            return services;
        }

//         private static IAsyncPolicy<HttpResponseMessage> GetRetryBackoffPolicy(IServiceCollection services,
//             RetryPolicyConfig config)
//         {
//             
//             var fallbackRetryPolicy = Policy
//                 .Handle<Exception>(x =>
//                 {
//                     
//                     Trace.TraceWarning($"Forever retrying after: {x.Message}");
//                     return true;
//                 })
//                 .RetryForeverAsync(onRetry: (ex, retryAttempt, _) =>
//                 {
//                     Trace.TraceWarning($"Making retry {retryAttempt}");
//                     // var warn = $"Delaying for {config.FallbackRetryInterval}, then making retry {retryAttempt}.";
//                     // await Task.Delay(config.FallbackRetryInterval).ConfigureAwait(false);
//                     // Trace.TraceWarning($"Http request timeout: {ex.Message}");
//
//                 });
//             
//             var backoffRetry = HttpPolicyExtensions
//                 .HandleTransientHttpError()
//                 .Or<Exception>(ex =>
//                 {
//                     var match = ex.Message.Contains("The request was canceled due to the configured HttpClient.Timeout", StringComparison.InvariantCultureIgnoreCase);
//                     if (match)
//                     {
//                         Trace.TraceWarning($"Http request timeout: {ex.Message}");
//                     }
//                     else
//                     {
//                         match = ex.InnerException != null &&
//                                     ex.InnerException.Message.Contains("Unable to read data from the transport connection", StringComparison.InvariantCultureIgnoreCase);
//                         if (match) Trace.TraceWarning($"IO exception occurred: {ex.InnerException.Message}");
//                     }
//                     return match;
//                 })
//                 .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
//                 .WaitAndRetryAsync(config.MaxRetryAttempts, 
//                     retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
//                         onRetry: (outcome, timespan, retryAttempt, context) =>
//                         {
//                             Trace.TraceWarning($"Delaying for {timespan.TotalMilliseconds}ms, then making retry {retryAttempt}.");
//                         });
//             return fallbackRetryPolicy
// //                .WrapAsync(circuitBreaker)
//                 .WrapAsync(backoffRetry);
//         }
        
    }
}