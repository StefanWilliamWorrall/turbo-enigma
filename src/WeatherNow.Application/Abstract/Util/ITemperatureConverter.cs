using WeatherNow.Domain.Forecast;

namespace WeatherNow.Application.Abstract.Util;

public interface ITemperatureConverter
{
    Temperature ConvertToCelsius(Temperature source);
}