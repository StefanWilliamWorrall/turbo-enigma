namespace WeatherNow.Domain.Geo;

/// <summary>
/// A location described by Latitude and Longitude Co-ordinates
/// </summary>
public record Location
{
    /// <summary>
    /// The Latitude Co-ordinate
    /// </summary>
    public double Latitude  { get; init; }
    /// <summary>
    /// The Longitude Co-ordinate
    /// </summary>
    public double Longitude  { get; init; }
}