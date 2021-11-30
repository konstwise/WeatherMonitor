using System.Linq;
using System.Threading.Tasks;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Core
{
    /// <summary>
    /// Implements <c>IForecastChecker</c> for Open Weather public API https://openweathermap.org/api
    /// </summary>
    public class ForecastChecker: IForecastChecker
    {
        private readonly IForecastProvider _forecastProvider;

        public ForecastChecker(IForecastProvider forecastProvider)
        {
            _forecastProvider = forecastProvider;
        }
        
        /// <inheritdoc cref="IForecastChecker"/>>
        public async Task<DailyForecastCheckResult[]> CheckLocationForecastAsync(LocationConfig location)
        {
            var result = await _forecastProvider.GetNextFiveDaysForecast(
                location.Name, location.CountryOrState);
            
            return ApplyLimits(result, location.Limits.LowerCelsius, location.Limits.UpperCelsius);
        }
        
        private static DailyForecastCheckResult[] ApplyLimits(
            DailyTemperatureRangeForecast[] forecasts, decimal minCelsius, decimal maxCelsius)
        {
            return forecasts
                .Select(rangeForecast =>
                    new DailyForecastCheckResult
                    {
                        Date = rangeForecast.Date,
                        IsUpperLimitExceeded = rangeForecast.MaxTemperatureCelsius > (double) maxCelsius,
                        IsLowerLimitExceeded = rangeForecast.MinTemperatureCelsius < (double) minCelsius
                    }).ToArray();
        }
    }
}
