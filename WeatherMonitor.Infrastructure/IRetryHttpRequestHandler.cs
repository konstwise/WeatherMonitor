using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherMonitor.Infrastructure
{
    public interface IRetryHttpRequestHandler
    {
        Task<HttpResponseMessage> HandleWithPolicyAsync(Func<Task<HttpResponseMessage>> sendAsync);
    }
}