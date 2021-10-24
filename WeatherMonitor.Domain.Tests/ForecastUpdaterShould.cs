using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherMonitor.Domain.Entities;
using Xunit;

namespace WeatherMonitor.Domain.Tests
{
    public class ForecastUpdaterTests
    {
        private Mock<IForecastProvider> _providerMock = new();

        private Mock<ILogger<Core.ForecastUpdater>> _loggerMock = new();

        private MonitoringConfig _config = new MonitoringConfig
        {
            Locations = Array.Empty<LocationConfig>()
        };

        private Mock<IForecastRepository> _repositoryMock = new(); 
        
        public class Constructor: ForecastUpdaterTests
        {
            [Fact]
            public void Throws_When_Null_Config_Provided()
            {
                Assert.Throws<ArgumentNullException>(() => new Core.ForecastUpdater(
                    _providerMock.Object, _loggerMock.Object, null, _repositoryMock.Object));
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
            public async Task Request_Forecast_For_Each_Location_In_Config()
            {
                var sut = new Core.ForecastUpdater(_providerMock.Object, _loggerMock.Object, _config, _repositoryMock.Object);
                await sut.UpdateAllLocationsAsync(CancellationToken.None);

                _providerMock.Verify(m => m.GetLocationForecastAsync(It.IsAny<LocationConfig>()),
                    Times.Exactly(3));
            }
            
            [Fact]
            public async Task Save_Forecast_For_Each_Location_In_Config()
            {
                var sut = new Core.ForecastUpdater(_providerMock.Object, _loggerMock.Object, _config, _repositoryMock.Object);
                await sut.UpdateAllLocationsAsync(CancellationToken.None);

                _repositoryMock.Verify(m => m.Update(It.IsAny<Location>(), It.IsAny<DailyTemperatureForecast[]>()),
                    Times.Exactly(3));
            }
        }
    }
}