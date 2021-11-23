using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherMonitor.Domain;
using WeatherMonitor.Domain.Entities;
using Xunit;

namespace WeatherMonitor.Core.Tests
{
    public class ForecastUpdaterTests
    {
        private readonly Mock<IForecastChecker> _providerMock = new();
        private readonly Mock<ILogger<Core.ForecastUpdater>> _loggerMock = new();
        private readonly Mock<IForecastRepository> _repositoryMock = new(); 
        private MonitoringConfig _config = new()
        {
            Locations = Array.Empty<LocationConfig>()
        };

        
        public class Constructor: ForecastUpdaterTests
        {
            [Fact]
            public void Throws_When_Null_Config_Specified()
            {
                Assert.Throws<ArgumentNullException>(() => new Core.ForecastUpdater(
                    _providerMock.Object, _loggerMock.Object, null, _repositoryMock.Object));
            }
            
            [Fact]
            public void Throws_When_Null_Forecast_Provider_Specified()
            {
                Assert.Throws<ArgumentNullException>(() => new Core.ForecastUpdater(
                    null, _loggerMock.Object, _config, _repositoryMock.Object));
            }
        
            [Fact]
            public void Throws_When_Null_Logger_Specified()
            {
                Assert.Throws<ArgumentNullException>(() => new Core.ForecastUpdater(
                    _providerMock.Object, null, _config, _repositoryMock.Object));
            }
        
            [Fact]
            public void Throws_When_Null_Repository_Specified()
            {
                Assert.Throws<ArgumentNullException>(() => new Core.ForecastUpdater(
                    _providerMock.Object, _loggerMock.Object, _config, null));
            }
            
        }

        public class UpdateAllLocationsAsync: ForecastUpdaterTests
        {
            public UpdateAllLocationsAsync()
            {
                _config = new MonitoringConfig()
                {
                    Locations = new[]
                    {
                        new LocationConfig {Name = "City 1"},
                        new LocationConfig {Name = "City 2"},
                        new LocationConfig {Name = "City 3"}
                    }
                };
            }
            
            [Fact]
            public async Task Requests_Forecast_For_Each_Location_In_Config()
            {
                var sut = new Core.ForecastUpdater(_providerMock.Object, _loggerMock.Object, _config, _repositoryMock.Object);
                await sut.UpdateAllLocationsAsync(CancellationToken.None);

                _providerMock.Verify(m => m.CheckLocationForecastAsync(It.IsAny<LocationConfig>()),
                    Times.Exactly(3));
            }
            
            [Fact]
            public async Task Updates_Forecast_For_Each_Location_In_Config()
            {
                var sut = new Core.ForecastUpdater(_providerMock.Object, _loggerMock.Object, _config, _repositoryMock.Object);
                await sut.UpdateAllLocationsAsync(CancellationToken.None);

                _repositoryMock.Verify(m => m.UpdateLocationForecast(It.IsAny<Location>(), It.IsAny<DailyForecastCheckResult[]>()),
                    Times.Exactly(3));
            }
        }
    }
}