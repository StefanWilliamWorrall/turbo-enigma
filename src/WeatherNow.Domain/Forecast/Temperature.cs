using System.Text.Json.Serialization;
using WeatherNow.Domain.Units;

namespace WeatherNow.Domain.Forecast;

public record Temperature
{
    public required double Value  { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required TemperatureUnit Unit  { get; set; }
}