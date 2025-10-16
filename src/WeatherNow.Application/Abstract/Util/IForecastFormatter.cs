using WeatherNow.Domain.Forecast;

namespace WeatherNow.Application.Abstract.Util;

public interface IForecastFormatter
{
    WeatherForecast Format(WeatherForecast forecast);
}