using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Geo;
using WeatherNow.Domain.Units;

namespace WeatherNow.Infrastructure.Tests.Helpers;

public class ForecastBuilder
{
    private Temperature? _temperature;
    private WindCondition? _wind;
    private Location? _location;
    private string? _condition;
    private string? _description;
    
    private ForecastBuilder()
    {
            
    }

    public ForecastBuilder WithDefaults()
    {
        _temperature = new Temperature
        {
            Value = 298.48,
            Unit = TemperatureUnit.Kelvin
        };

        _wind = new WindCondition
        {
            Speed = 0.62,
            Unit = SpeedUnit.MetersPerSecond
        };

        _location = new Location
        {
            Latitude = 44.34,
            Longitude = 10.99,
        };
        
        _condition = "Rain";

        _description = "moderate rain";

        return this;
    }


    public WeatherForecast Build()
    {
        var forecast = new WeatherForecast
        {
            Temperature = _temperature
            , Conditions = _condition
            , WindCondition = _wind
            , Description = _description
            ,  ForecastLocation= _location
        };
        
        return forecast;
    }
    public static ForecastBuilder Create() => new();
}