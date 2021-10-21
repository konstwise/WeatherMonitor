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
            services.Configure<OpenWeatherMapApiConfig>(configuration.GetSection(
                nameof(OpenWeatherMapApiConfig)));
            services.AddHttpClient<IForecastProvider, OpenWeatherMapForecastProvider>(c =>
            {
                c.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
            });            
            return services;
        }
    }
}