using System.Text.Json.Serialization;
using WeatherNow.Domain.Units;

namespace WeatherNow.Domain.Forecast;

public record WindCondition
{
    public required double Speed   { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required SpeedUnit Unit   { get; set; }
}