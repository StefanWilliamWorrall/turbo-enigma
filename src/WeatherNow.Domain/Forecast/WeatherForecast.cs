using WeatherNow.Domain.Geo;

namespace WeatherNow.Domain.Forecast;

/// <summary>
/// The weather forecast
/// </summary>
public record WeatherForecast
{
    public required Location ForecastLocation { get; set; } 
    
    /// <summary>
    /// The description of the forecast
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// The short weather conditions description
    /// </summary>
    /// <remarks>
    /// The high level condition e.g. Rain, Windy, Rainy
    /// </remarks>
    public required string Conditions  { get; set; }

    /// <summary>
    /// The current temperature in Kelvin
    /// </summary>
    public required Temperature Temperature  { get; set; }

    /// <summary>
    /// The current Wind Speed
    /// </summary>
    public required WindCondition WindCondition  { get; set; }

    /// <summary>
    /// A recommendation based on the weather forecast
    /// </summary>
    public string Recommendation  { get; set; }
}