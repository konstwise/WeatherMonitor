using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherMonitor.Domain;
using Microsoft.Extensions.Hosting;
using WeatherMonitor.OpenWeatherMapProvider;

namespace WeatherMonitor.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeatherMonitor(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<IForecastRepository, ForecastRepository>();
            services.Configure<MonitoringConfig>(configuration.GetSection(
                nameof(MonitoringConfig)));
           services.AddOpenWeatherMapProvider(configuration);

           return services;
        }
    }
}