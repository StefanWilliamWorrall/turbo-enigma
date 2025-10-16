using WeatherNow.Application.Abstract.Util;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Units;

namespace WeatherNow.Application.Util;

public class WindSpeedConvertor : IWindSpeedConverter
{
    public WindCondition ConvertToKilometersPerHour(WindCondition windCondition)
    {
        if (windCondition.Unit == SpeedUnit.KilometersPerHour)
        {
            return windCondition;
        }

        var convertedConditions = new WindCondition
        {
            Speed = windCondition.Speed * 3.6,
            Unit = SpeedUnit.KilometersPerHour
        };
        
        return  convertedConditions;
    }
}