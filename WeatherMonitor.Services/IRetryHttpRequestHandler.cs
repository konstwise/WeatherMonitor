using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherMonitor.Services
{
    public interface IRetryHttpRequestHandler
    {
        Task<HttpResponseMessage> HandleWithPolicyAsync(Func<Task<HttpResponseMessage>> sendAsync);
    }
}