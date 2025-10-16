namespace WeatherNow.Infrastructure.Services;

/// <summary>
/// The config for the Open Weather Map Config Section in the App Config
/// </summary>
public record OpenWeatherMapConfig
{
    /// <summary>
    /// The Api Key to make calls to the Open Map Weather Api
    /// </summary>
    public string? ApiKey { get; init; }
    /// <summary>
    /// The Base URL for the Weather Api
    /// </summary>
    public string? BaseApiUrl { get; init; }
}