using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherMonitor.OpenWeatherMapProvider
{
    public interface IRetryHttpRequestSender
    {
        Task<HttpResponseMessage> HandleRequestWithPolicyAsync(Func<Task<HttpResponseMessage>> sendAsync);
    }
}