using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherMonitor.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            services.AddSingleton<IForecastUpdater, ForecastUpdater.ForecastUpdater>();
            
            services.Configure<MonitoringConfig>(configuration.GetSection(
                nameof(MonitoringConfig)));
           services.AddOpenWeatherMapProvider(configuration);

           return services;
        }        
        public static IServiceCollection AddForecastUpdater(this IServiceCollection services)
        {
           services.AddHostedService(sp => new LongRunningTaskExecutor(
               (ct)=> 
                        sp.GetRequiredService<IForecastUpdater>().UpdatePeriodicallyAsync(ct),
                    sp.GetRequiredService<ILogger<LongRunningTaskExecutor>>()
               ));

           return services;
        }
    }
}