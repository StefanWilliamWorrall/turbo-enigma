using NSubstitute;
using WeatherNow.Application.Abstract.Util;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Units;

namespace WeatherNow.Application.Tests.Helpers;

public class TemperatureConverterBuilder
{

    private Temperature? _response = null;
    
    private TemperatureConverterBuilder()
    {
            
    }

    public TemperatureConverterBuilder WithDefaults()
    {
        _response = new Temperature
        {
            Unit = TemperatureUnit.Celsius,
            Value = 18.9
        };

        return this;
    }

    public ITemperatureConverter Build()
    {
        var convertor = Substitute.For<ITemperatureConverter>();
        convertor.ConvertToCelsius(Arg.Any<Temperature>())
            .Returns(_response);
        
        return convertor;
    }

    public (ITemperatureConverter, Temperature?) BuildAndReturnExpected()
    {
        var converter = Build();
        return (converter, _response);
    }
    
    public static TemperatureConverterBuilder Create() => new();
    
}