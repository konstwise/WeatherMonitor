using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherMonitor.Domain;
using WeatherMonitor.OpenWeatherMapProvider;
using WeatherMonitor.Services;

namespace WeatherMonitor.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeatherMonitor(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IForecastRepository, InMemoryForecastRepository.InMemoryForecastRepository>();
            services.AddSingleton<IExecutor, ForecastUpdater.ForecastUpdater>();

            MonitoringConfig config = new();
            configuration.GetSection(
                    nameof(MonitoringConfig))
                .Bind(config);

            services.AddSingleton(config);
            services.AddOpenWeatherMapProvider(configuration);
            return services;
        }

        public static IServiceCollection AddRecurringTaskExecutor(this IServiceCollection services)
        {
            services.AddHostedService<RecurringTaskExecutor>();

            return services;
        }
    }
}