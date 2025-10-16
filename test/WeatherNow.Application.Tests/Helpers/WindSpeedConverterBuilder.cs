using NSubstitute;
using WeatherNow.Application.Abstract.Util;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Units;

namespace WeatherNow.Application.Tests.Helpers;

public class WindSpeedConverterBuilder
{
    private double _value;
    private SpeedUnit _unit;
    private WindSpeedConverterBuilder()
    {
            
    }

    public WindSpeedConverterBuilder WithDefaults()
    {
        _value = 10;
        _unit = SpeedUnit.KilometersPerHour;
        return this;
    }

    public IWindSpeedConverter Build() => BuildAndReturnExpected().Item1;
    
    public (IWindSpeedConverter, WindCondition) BuildAndReturnExpected()
    {
        var service = Substitute.For<IWindSpeedConverter>();
        var convertResult = new WindCondition
        {
            Speed = _value,
            Unit = _unit
        };
        service.ConvertToKilometersPerHour(Arg.Any<WindCondition>())
            .Returns(convertResult);
        
        return (service, convertResult);
    }

    public static WindSpeedConverterBuilder Create() => new();
}