using System.Net;

namespace WeatherNow.Infrastructure.Tests.Helpers;

public class TestHttpMessageHandler : HttpMessageHandler
{
    public HttpRequestMessage? LastRequest { get; private set; }
    public Func<HttpRequestMessage, HttpResponseMessage>? Handler { get; set; }
    
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request
        , CancellationToken cancellationToken)
    {
        LastRequest = request;
        var response = Handler?.Invoke(LastRequest) ?? new HttpResponseMessage(HttpStatusCode.NoContent);
        return Task.FromResult(response);
    }
}