using WeatherNow.Domain.Forecast;

namespace WeatherNow.Application.Abstract.Util;

public interface IWindSpeedConverter
{
    WindCondition ConvertToKilometersPerHour(WindCondition windCondition);
}