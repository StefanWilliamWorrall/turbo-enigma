using System.Text;
using WeatherNow.Application.Abstract.Util;
using WeatherNow.Domain.Forecast;

namespace WeatherNow.Application.Util;

public class ForecastFormatter(
    ITemperatureConverter temperatureConverter
    , IWindSpeedConverter windSpeedConverter
    ) : IForecastFormatter
{
    public WeatherForecast Format(WeatherForecast forecast)
    {
        var formattedTemperature = temperatureConverter.ConvertToCelsius(forecast.Temperature);
        var formattedWindCondition = windSpeedConverter.ConvertToKilometersPerHour(forecast.WindCondition);
        var weatherRecommendation = GetRecommendationForForecast(forecast.Conditions, formattedTemperature);

        var formattedForecast = forecast with
        {
            WindCondition = formattedWindCondition
            , Temperature = formattedTemperature
            , Recommendation = weatherRecommendation
        };
        
        return formattedForecast;
    }

    public string GetRecommendationForForecast(
        string forecastCondition
        , Temperature currentTemperature
    )
    {
        StringBuilder recommendationBuilder = new StringBuilder();
        //Clear Sky - Surely Sunny day
        if (string.Compare(forecastCondition, "Clear", StringComparison.InvariantCultureIgnoreCase) == 0)
        {
           AppendRecommendation("Don't forget to bring a hat.");
        }
        if (currentTemperature.Value > 25)
        {
            AppendRecommendation("It’s a great day for a swim.");
        }

        var isRaining = string.Compare(forecastCondition, "Rain", StringComparison.InvariantCultureIgnoreCase) == 0;
        if (currentTemperature.Value < 15 &&  (
                    isRaining
                     ||string.Compare(forecastCondition, "Snow", StringComparison.InvariantCultureIgnoreCase) == 0
                     || string.Compare(forecastCondition, "Drizzle", StringComparison.InvariantCultureIgnoreCase) == 0
                     ))
        {
            AppendRecommendation("Don't forget to bring a coat.");
        }

        if (isRaining)
        {
            AppendRecommendation("Don't forget the umbrella.");
        }
        
        return recommendationBuilder.ToString();
        
        void AppendRecommendation(string recommendation)
        {
            if (recommendationBuilder.Length > 0)
                recommendationBuilder.Append(' ');
            recommendationBuilder.Append(recommendation);
        }
    }
}