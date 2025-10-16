using NSubstitute;
using WeatherNow.Application.Abstract.Util;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Geo;
using WeatherNow.Domain.Units;

namespace WeatherNow.Application.Tests.Helpers;

public class ForecastFormatterBuilder
{
    private WeatherForecast? _formattedForecast;
    private ForecastFormatterBuilder()
    {
        
    }

    public ForecastFormatterBuilder WithDefaults()
    {
        _formattedForecast = new WeatherForecast
        {
            Conditions = "Rain",
            Description = "moderate rain",
            Temperature = new Temperature
            {
                Value = 11.05,
                Unit = TemperatureUnit.Celsius
            },
            WindCondition = new WindCondition
            {
                Speed = 14.724,
                Unit = SpeedUnit.KilometersPerHour
            },
            Recommendation = "Don’t forget the umbrella",
            ForecastLocation = new Location
            {
                Latitude = 1.0,
                Longitude = 1.0
            }
        };
        return this;
    }
    

    public IForecastFormatter Build()
    {
        var formatter = Substitute.For<IForecastFormatter>();
        formatter.Format(Arg.Any<WeatherForecast>()).Returns(_formattedForecast);
        
        return formatter;
    }

    public (IForecastFormatter, WeatherForecast?) BuildFromDefault()
    {
        var formatter = Build();
        return (formatter, _formattedForecast);
    }
    
    public static ForecastFormatterBuilder Create() => new();
}