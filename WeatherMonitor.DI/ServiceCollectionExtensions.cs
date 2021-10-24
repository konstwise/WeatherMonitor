using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherMonitor.Core;
using WeatherMonitor.Domain;
using WeatherMonitor.OpenWeatherMapProvider;
using WeatherMonitor.Infrastructure;
using WeatherMonitor.KeyValueStore;

namespace WeatherMonitor.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeatherMonitorCore(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IForecastCheckResultsRepository, ForecastCheckResultsRepository>();
            services.AddSingleton(typeof(IKeyValueStore<,>), typeof(InMemoryKeyValueStore<,>));
            services.AddSingleton<IForecastCheckResultsUpdater, ForecastCheckResultsUpdater>();
            services.AddSingleton<ILongRunningTaskSource, ForecastCheckMonitor>();

            MonitoringConfig config = new();
            configuration.GetSection(
                    nameof(MonitoringConfig))
                .Bind(config);

            services.AddSingleton(config);
            
            services.AddOpenWeatherMapProvider(configuration);
            
            var retryPolicyConfig = configuration.GetSection(
                    nameof(RetryPolicyConfig))
                .Get<RetryPolicyConfig>();

            services.AddSingleton(retryPolicyConfig);
            services.AddSingleton<IRetryHttpRequestHandler, RetryHttpRequestHandler>();
            
            return services;
        }

        public static IServiceCollection AddLongRunningTaskExecutor(this IServiceCollection services)
        {
            return services.AddHostedService<BackgroundWorker>();
        }
    }
}