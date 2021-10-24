using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;

namespace WeatherMonitor.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IForecastCheckResultsRepository _forecastCheckResultsRepository;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IForecastCheckResultsRepository forecastCheckResultsRepository, ILogger<WeatherForecastController> logger)
        {
            _forecastCheckResultsRepository = forecastCheckResultsRepository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<LocationForecastCheckResults> Get()
        {
            return _forecastCheckResultsRepository.GetAllLocationCheckResults();
        }
    }
}