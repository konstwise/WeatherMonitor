using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherMonitor.Domain;
using WeatherMonitor.ForecastUpdater.Tests;

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
            services.AddHttpClient<IForecastProvider, OpenWeatherMapForecastProvider>(c =>
            {
                c.BaseAddress = new Uri(config.BaseUrl);
            });
            
            return services;
        }
    }
}