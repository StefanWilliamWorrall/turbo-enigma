using System.Net;
using Newtonsoft.Json;
using WeatherNow.Infrastructure.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WeatherNow.Infrastructure.Tests.Helpers;

public class HttpTestClientBuilder
{
    private TestHttpMessageHandler _testHandler;
    private string _baseAddress = "http://localhost";
    private HttpTestClientBuilder()
    {
            
    }

    public HttpTestClientBuilder WithDefaults()
    {
        _testHandler = new TestHttpMessageHandler();
        return this;
    }

    public HttpTestClientBuilder WithSuccessResponseObject(string responseFileName)
    {
        var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "SampleResponse", responseFileName);
        var responseContent = File.ReadAllText(filePath);
        
        _testHandler = new TestHttpMessageHandler();
        _testHandler.Handler = _ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(responseContent)
        };

        return this;
    }

    public HttpTestClientBuilder WithFailureResponse()
    {
        _testHandler = new TestHttpMessageHandler();
        _testHandler.Handler = _ => new HttpResponseMessage(HttpStatusCode.InternalServerError);
        return this;
    }

    public HttpClient Build()
    {
        var client = new HttpClient(_testHandler);
        client.BaseAddress = new Uri(_baseAddress);
        return client;
    }

    public (HttpClient, TestHttpMessageHandler) BuildAndReturnTestHandler()
    {
        var client = Build();
        return (client, _testHandler);
    }
    
    public HttpTestClientBuilder WithBaseAddress(OpenWeatherMapConfig openWeatherMapConfig)
    {
        _baseAddress = openWeatherMapConfig.BaseApiUrl;
        return this;
    }
    


    public static HttpTestClientBuilder Create() => new();


}