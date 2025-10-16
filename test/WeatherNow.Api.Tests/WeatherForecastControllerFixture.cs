using Arta.Abstractions.Dispatch;
using Arta.Abstractions.Requests;
using Arta.Abstractions.Results;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WeatherNow.Api.Controllers;
using WeatherNow.Application.Queries.Forecast;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Geo;
using WeatherNow.Domain.Units;

namespace WeatherNow.Api.Tests;

[TestFixture]
public class WeatherForecastControllerFixture
{
    [Test]
    public async Task Get_ShouldReturnBadRequestWithErrorResultWhenQueryFails()
    {
        var latitude = 51.5074;
        var longitude = -0.1278;
        
        var logger = Substitute.For<ILogger<WeatherForecastController>>();
        var dispatcher = Substitute.For<IDispatcher>();

        var controller = new WeatherForecastController(dispatcher, logger);

        var error = new QueryFailureResult<WeatherForecast>(
            "Invalid coordinates"
            , Substitute.For<IRequestContext>()
        );
        
        dispatcher
            .SendQueryAsync<GetCurrentForecastQuery, WeatherForecast>(Arg.Any<GetCurrentForecastQuery>())
            .Returns(error);
        
        var result = await controller.Get(latitude, longitude);
        
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = result as BadRequestObjectResult;
        badRequest!.Value.Should().Be(error);
    }
    
    [Test]
    public async Task Get_ShouldReturnInternalServerErrorWhenDispatcherThrowsException()
    {
        
        var latitude = 10.0;
        var longitude = 20.0;
        var exceptionMessage = "Unexpected error";

        var logger = Substitute.For<ILogger<WeatherForecastController>>();
        var dispatcher = Substitute.For<IDispatcher>();

        var controller = new WeatherForecastController(dispatcher, logger);
        
        dispatcher
            .SendQueryAsync<GetCurrentForecastQuery, WeatherForecast>(Arg.Any<GetCurrentForecastQuery>())
            .Throws(new InvalidOperationException(exceptionMessage));
        
        var result = await controller.Get(latitude, longitude);
        
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(500);

        objectResult.Value.Should().BeEquivalentTo(new
        {
            message = exceptionMessage
        });
    }
    
    [Test]
    public async Task Get_ShouldReturnOkWithForecastWhenQuerySucceeds()
    {
        // Arrange
        var latitude = 40.7128;
        var longitude = -74.0060;
        
        var logger = Substitute.For<ILogger<WeatherForecastController>>();
        var dispatcher = Substitute.For<IDispatcher>();

        var controller = new WeatherForecastController(dispatcher, logger);

        var forecast = new WeatherForecast
        {
            Description = "Sunny",
            Conditions = "Clear",
            Temperature = new Temperature { Value = 25, Unit = TemperatureUnit.Celsius },
            WindCondition = new WindCondition { Speed = 10, Unit = SpeedUnit.KilometersPerHour },
            Recommendation = "Go for a walk.",
            ForecastLocation = new Location { Latitude = latitude, Longitude = longitude },
        };

        var queryResult = new QuerySuccessResult<WeatherForecast>(
            forecast
            , Substitute.For<IRequestContext>()
        );

        dispatcher
            .SendQueryAsync<GetCurrentForecastQuery, WeatherForecast>(Arg.Any<GetCurrentForecastQuery>())
            .Returns(queryResult);

        // Act
        var result = await controller.Get(latitude, longitude);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var ok = result as OkObjectResult;
        ok!.Value.Should().BeEquivalentTo(forecast);

       _ = await dispatcher.Received(1)
            .SendQueryAsync<GetCurrentForecastQuery, WeatherForecast>(Arg.Is<GetCurrentForecastQuery>(q =>
                q.ForecastLocation.Latitude == latitude &&
                q.ForecastLocation.Longitude == longitude));
    }
}