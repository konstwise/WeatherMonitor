using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherMonitor.Domain;
using Xunit;

namespace WeatherMonitor.Core.Tests
{
    public class ForecastCheckMonitorTests
    {
        private readonly Mock<IForecastCheckResultsUpdater> _updaterMock = new();
        private readonly Mock<ILogger<ForecastCheckMonitor>> _loggerMock = new();
        private MonitoringConfig _config = new()
        {
            Locations = Array.Empty<LocationConfig>()
        };

        
        public class Constructor: ForecastCheckMonitorTests
        {
            [Fact]
            public void Throws_When_Null_Config_Specified()
            {
                Assert.Throws<ArgumentNullException>(() => new ForecastCheckMonitor(
                    _updaterMock.Object, null, _loggerMock.Object));
            }
            
            [Fact]
            public void Throws_When_Null_Forecast_Updater_Specified()
            {
                Assert.Throws<ArgumentNullException>(() => new ForecastCheckMonitor(
                    null, _config, _loggerMock.Object));
            }
        
            [Fact]
            public void Throws_When_Null_Logger_Specified()
            {
                Assert.Throws<ArgumentNullException>(() => new ForecastCheckMonitor(
                    _updaterMock.Object, _config, null));
            }
        }

        public class UpdateAllLocationsAsync: ForecastCheckMonitorTests
        {
            public UpdateAllLocationsAsync()
            {
                _config = new MonitoringConfig()
                {
                    UpdateInterval = TimeSpan.FromMilliseconds(500),
                    Locations = new[]
                    {
                        new LocationConfig {Name = "City 1"},
                        new LocationConfig {Name = "City 2"},
                        new LocationConfig {Name = "City 3"}
                    }
                };
            }
            
            [Fact]
            public async Task Calls_Updater_Periodically()
            {
                var cts = new CancellationTokenSource();
                var stoppingToken = CancellationTokenSource.CreateLinkedTokenSource(cts.Token).Token;
 
                // Setup total execution time for 3 calls and 100ms timeout before stopping
                cts.CancelAfter(300 + (int)(_config.UpdateInterval.TotalMilliseconds * 2)) ;

                try      
                {
                    var sut = new ForecastCheckMonitor(_updaterMock.Object, _config, _loggerMock.Object);
                    await sut.UpdateForecastCheckResultsPeriodicallyAsync(stoppingToken);
                }
                catch (TaskCanceledException) { /* OK to throw when delaying token expires */}

                _updaterMock.Verify(m => m.UpdateAllLocationsAsync(stoppingToken),
                    Times.Exactly(3));
            }
        }
    }
}