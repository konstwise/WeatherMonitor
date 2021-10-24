using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;
using WeatherMonitor.ForecastUpdater.Tests;

namespace WeatherMonitor.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IForecastRepository _forecastRepository;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IForecastRepository forecastRepository, ILogger<WeatherForecastController> logger)
        {
            _forecastRepository = forecastRepository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<LocationForecast> Get()
        {
            return _forecastRepository.GetAllLocationForecasts();
        }
    }
}