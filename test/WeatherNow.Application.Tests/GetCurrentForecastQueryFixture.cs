using Arta.Abstractions.Results;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WeatherNow.Application.Queries.Forecast;
using WeatherNow.Application.Tests.Helpers;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Geo;

namespace WeatherNow.Application.Tests;

[TestFixture]
public class GetCurrentForecastQueryFixture
{
    
    [Test]
    public async Task HandleAsync_ShouldReturnErrorIfServiceHasError()
    {
        const string forecastServiceErrorMessage = "Error from Service";
        var fakeForecastService = ForecastServiceBuilder.Create()
            .ProducesFailedResponse(errorMessage: forecastServiceErrorMessage)
            .Build();

        var fakeRequestContext = RequestContextBuilder.Create()
            .WithDefaults()
            .Build();

        var fakeForecastFormatter = ForecastFormatterBuilder.Create()
            .WithDefaults()
            .Build();
        
        var fakeLogger = Substitute.For<ILogger<GetCurrentForecastQueryHandler>>();
        
        
        var handler = new GetCurrentForecastQueryHandler(fakeForecastService, fakeForecastFormatter, fakeLogger);
        var actual = await handler.HandleAsync(new GetCurrentForecastQuery
        {
            ForecastLocation = new Location(),

        }, fakeRequestContext);
        
        actual.Should().BeOfType(typeof(QueryFailureResult<WeatherForecast>));
        actual.As<QueryFailureResult<WeatherForecast>>().ErrorMessage.Should().Be(forecastServiceErrorMessage);
    }
    
    [Test]
    public async Task HandleAsync_ShouldReturnCancelledWhenTokenRequestsCancellation()
    {

        var fakeLogger = Substitute.For<ILogger<GetCurrentForecastQueryHandler>>();
        var fakeForecastFormatter = ForecastFormatterBuilder.Create()
            .WithDefaults()
            .Build();
        var fakeForecastService = ForecastServiceBuilder.Create()
            .ProducesCancelledResponse()
            .Build();
        var fakeContext = RequestContextBuilder.Create()
            .WithDefaults()
            .WithCancellationTokenCancelled()
            .Build();
        
        var handler = new GetCurrentForecastQueryHandler(fakeForecastService, fakeForecastFormatter, fakeLogger);
        var actual = await handler.HandleAsync(new GetCurrentForecastQuery
        {
            ForecastLocation = new Location(),

        }, fakeContext);
        
        actual.Should().BeOfType(typeof(QueryCancelledResult<WeatherForecast>));
    }
    
    [Test]
    public async Task HandleAsync_ShouldReturnForecastFromService()
    {
        var fakeLogger = Substitute.For<ILogger<GetCurrentForecastQueryHandler>>();
        var fakeContext = RequestContextBuilder.Create()
            .WithDefaults()
            .Build();

        var (fakeForecastFormatter, expected) = ForecastFormatterBuilder.Create()
            .WithDefaults()
            .BuildFromDefault();

        var fakeForecastService = ForecastServiceBuilder.Create()
            .ProducesSuccessfulResponseWithDefaults()
            .Build();
        
        
        var handler = new GetCurrentForecastQueryHandler(fakeForecastService, fakeForecastFormatter,  fakeLogger);
        var actual = await handler.HandleAsync(new GetCurrentForecastQuery
        {
            ForecastLocation = new Location(),

        }, fakeContext);
        
        actual.Should().BeOfType(typeof(QuerySuccessResult<WeatherForecast>));
        actual.As<QuerySuccessResult<WeatherForecast>>().Result.Should().BeSameAs(expected);
        
        //Ensure we are converting the weather forecast to the correct units and structure for web api response
        _ = fakeForecastFormatter.Received(1).Format(Arg.Any<WeatherForecast>());

    }
    
    [Test]
    public async Task HandleAsync_ShouldCatchAnyErrorAndReturnFailedResult()
    {
        var fakeLogger = Substitute.For<ILogger<GetCurrentForecastQueryHandler>>();
        var fakeContext = RequestContextBuilder.Create()
            .WithDefaults()
            .Build();

        var (fakeForecastFormatter, expected) = ForecastFormatterBuilder.Create()
            .WithDefaults()
            .BuildFromDefault();

        var fakeForecastService = ForecastServiceBuilder.Create()
            .ThrowsException<InvalidOperationException>()
            .Build();
        
        
        var handler = new GetCurrentForecastQueryHandler(fakeForecastService, fakeForecastFormatter,  fakeLogger);
        var actual = await handler.HandleAsync(new GetCurrentForecastQuery
        {
            ForecastLocation = new Location(),

        }, fakeContext);
        
        actual.Should().BeOfType(typeof(QueryFailureResult<WeatherForecast>));
        
    }
}