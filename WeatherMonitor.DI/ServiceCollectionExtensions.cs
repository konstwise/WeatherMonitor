using System;
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
            services.AddSingleton<IForecastRepository, ForecastRepository>();
            services.AddSingleton(typeof(IKeyValueStore<,>), typeof(InMemoryKeyValueStore<,>));
            services.AddSingleton<IForecastChecker, ForecastChecker>();
            services.AddSingleton<IForecastUpdater, ForecastUpdater>();
            services.AddSingleton<ILongRunningTaskSource, ForecastMonitor>();

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
        
        public static IServiceCollection AddOpenWeatherMapProvider(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IForecastProvider, OpenWeatherMapForecastProvider>();
            OpenWeatherMapApiConfig config = new();
            configuration.GetSection(
                    nameof(OpenWeatherMapApiConfig))
                .Bind(config);

            services.AddSingleton(config);
            services.AddHttpClient<IForecastProvider, OpenWeatherMapForecastProvider>(c =>
            {
                c.BaseAddress = new Uri(config.BaseUrl);
            });
            
            return services;
        }

    }
}