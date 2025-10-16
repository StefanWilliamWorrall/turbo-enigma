using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WeatherNow.Application.Abstract.Services;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Geo;
using WeatherNow.Domain.Units;

namespace WeatherNow.Application.Tests.Helpers;

public class ForecastServiceBuilder
{
    private ServiceResult<WeatherForecast>? _serviceResult;
    private Exception? _exception;
    private ForecastServiceBuilder()
    {

    }

    public ForecastServiceBuilder ProducesFailedResponse(string errorMessage)
    {
            _serviceResult = new ServiceResult<WeatherForecast>
            {
                IsSuccessful = false,
                ErrorMessage = errorMessage
            };
            return this;
    }

    public ForecastServiceBuilder ProducesSuccessfulResponseWithDefaults()
    {
        _serviceResult = new ServiceResult<WeatherForecast>
        {
            IsSuccessful = true,
            Result = new WeatherForecast
            {
                Conditions = "Rain",
                Description = "moderate rain",
                Temperature = new Temperature
                {
                    Value = 284.2,
                    Unit = TemperatureUnit.Kelvin
                },
                WindCondition = new WindCondition
                {
                    Speed = 4.09,
                    Unit = SpeedUnit.MetersPerSecond
                },
                ForecastLocation = new Location
                {
                    Longitude = 1.0,
                    Latitude = 1.0
                }
            }
        };
        return this;
    }
    

    public ForecastServiceBuilder ProducesCancelledResponse()
    {
        _serviceResult = new ServiceResult<WeatherForecast>
        {
            IsSuccessful = false,
            IsCancelled = true,
            ErrorMessage = "Service has been cancelled"
        };

        return this;
    }

    public ForecastServiceBuilder ThrowsException<TException>() where TException : Exception, new()
    {
        _exception = new TException();
        return this;
    }
    

    public IForecastService Build()
    {
        var forecastService = Substitute.For<IForecastService>();
        if (_exception is null)
        {
            forecastService.GetCurrentForecast(
                    Arg.Any<Location>()
                    , Arg.Any<CancellationToken>()
                    )
                .Returns(Task.FromResult(_serviceResult!));
        }
        else
        {
            forecastService.GetCurrentForecast(
                    Arg.Any<Location>()
                    , Arg.Any<CancellationToken>()
                    )
                .ThrowsForAnyArgs(_exception);
        }
        

        return forecastService;
    }

    public static ForecastServiceBuilder Create()
    {
        return new ForecastServiceBuilder();
    }
}