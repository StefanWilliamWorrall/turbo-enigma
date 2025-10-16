using System.Net;
using System.Text.Json;
using System.Web;
using FluentAssertions;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Geo;
using WeatherNow.Infrastructure.Services;
using WeatherNow.Infrastructure.Tests.Helpers;

namespace WeatherNow.Infrastructure.Tests;

[TestFixture]
public class OpenWeatherMapServiceFixture
{
    private OpenWeatherMapConfig _openWeatherMapConfig = new OpenWeatherMapConfig
    {
        ApiKey = "test-api-key",
        BaseApiUrl = "http://test-api-url"
    };
    
    [Test]
    public async Task GetCurrentForecast_ShouldReturnSuccess_WhenApiReturnsOk()
    {
        var expectedForecast = ForecastBuilder.Create()
            .WithDefaults()
            .Build();

        var (testClient, testHandler) = HttpTestClientBuilder.Create()
            .WithBaseAddress(_openWeatherMapConfig)
            .WithSuccessResponseObject("SuccessSampleResponse.json")
            .BuildAndReturnTestHandler();

        var location = new Location
        {
            Latitude = -41.28664,
            Longitude = 174.77557
        };

        var forecastService = new OpenWeatherMapService(testClient, _openWeatherMapConfig);
        
        var actual = await forecastService.GetCurrentForecast(location, CancellationToken.None);
        
        actual.Should().NotBeNull();
        actual.IsSuccessful.Should().BeTrue();
        actual.Result.Should().BeEquivalentTo(expectedForecast);

        testHandler.LastRequest.Should().NotBeNull();

        var queryParts = HttpUtility.ParseQueryString(testHandler.LastRequest!.RequestUri!.Query);
        queryParts["appid"].Should().Be(_openWeatherMapConfig.ApiKey);
        queryParts["lat"].Should().Be(location.Latitude.ToString());
        queryParts["lon"].Should().Be(location.Longitude.ToString());
    }
    
    [Test]
    public async Task GetCurrentForecast_ShouldReturnServiceFailure_WhenNoResponseReceived()
    {
        var expectedForecast = ForecastBuilder.Create()
            .WithDefaults()
            .Build();

        var (testClient, testHandler) = HttpTestClientBuilder.Create()
            .WithBaseAddress(_openWeatherMapConfig)
            .WithFailureResponse()
            .BuildAndReturnTestHandler();

        var location = new Location
        {
            Latitude = -41.28664,
            Longitude = 174.77557
        };

        var forecastService = new OpenWeatherMapService(testClient, _openWeatherMapConfig);
        
        var actual = await forecastService.GetCurrentForecast(location, CancellationToken.None);
        
        actual.Should().NotBeNull();
        actual.IsSuccessful.Should().BeFalse();
        actual.IsSuccessful.Should().BeFalse();
        actual.Result.Should().BeNull();

        testHandler.LastRequest.Should().NotBeNull();

        var queryParts = HttpUtility.ParseQueryString(testHandler.LastRequest!.RequestUri!.Query);
        queryParts["appid"].Should().Be(_openWeatherMapConfig.ApiKey);
        queryParts["lat"].Should().Be(location.Latitude.ToString());
        queryParts["lon"].Should().Be(location.Longitude.ToString());
    }
    
    [Test]
    public async Task GetCurrentForecast_ShouldReturnServiceCancelled_WhenCancellationIsRequested()
    {

        var (testClient, testHandler) = HttpTestClientBuilder.Create()
            .WithBaseAddress(_openWeatherMapConfig)
            .WithFailureResponse()
            .BuildAndReturnTestHandler();

        var location = new Location
        {
            Latitude = -41.28664,
            Longitude = 174.77557
        };

        var forecastService = new OpenWeatherMapService(testClient, _openWeatherMapConfig);
        var testCancellation = new CancellationTokenSource();
        await testCancellation.CancelAsync();
        
        
        var actual = await forecastService.GetCurrentForecast(location, testCancellation.Token);
        
        actual.Should().NotBeNull();
        actual.IsSuccessful.Should().BeFalse();
        actual.IsCancelled.Should().BeTrue();
        actual.Result.Should().BeNull();
        
    }
}