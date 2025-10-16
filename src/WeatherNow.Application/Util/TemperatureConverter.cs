using WeatherNow.Application.Abstract.Util;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Units;

namespace WeatherNow.Application.Util;

public class TemperatureConverter : ITemperatureConverter
{
    public Temperature ConvertToCelsius(Temperature source)
    {
        if (source.Unit == TemperatureUnit.Celsius)
        {
            return source;
        }


        var convertedTempValue = source.Unit switch
        {
            TemperatureUnit.Celsius => source.Value,
            TemperatureUnit.Fahrenheit => FromFahrenheitToCelsius(source.Value),
            TemperatureUnit.Kelvin => FromKelvinToCelsius(source.Value),
            _ => throw new ArgumentOutOfRangeException(nameof(source.Unit))
        };

        var convertTemp = new Temperature
        {
            Value = convertedTempValue,
            Unit = TemperatureUnit.Celsius
        };

        return convertTemp;

    }

    private double FromFahrenheitToCelsius(double fahrenheit) => (fahrenheit - 32.0) * 5.0 / 9;
    private double FromCelsiusToFahrenheit(double celsius) => celsius * 9 / 5 + 32;
    private double FromKelvinToCelsius(double kelvin) => kelvin - 273.15;

}