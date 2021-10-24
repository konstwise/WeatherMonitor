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
            services.AddSingleton<IForecastChecker, OpenWeatherMapForecastChecker>();
            OpenWeatherMapApiConfig config = new();
            configuration.GetSection(
                    nameof(OpenWeatherMapApiConfig))
                .Bind(config);

            services.AddSingleton(config);
            services.AddHttpClient<IForecastChecker, OpenWeatherMapForecastChecker>(c =>
            {
                c.BaseAddress = new Uri(config.BaseUrl);
            });
            
            return services;
        }
    }
}