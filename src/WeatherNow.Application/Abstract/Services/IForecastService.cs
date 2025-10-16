using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Geo;

namespace WeatherNow.Application.Abstract.Services;

/// <summary>
/// The service that retrieves the weather forecast
/// </summary>
public interface IForecastService
{
   
   /// <summary>
   /// Retrieve the current weather at the specified location from the external weather source
   /// </summary>
   /// <param name="location">The location for the weather forecast</param>
   /// <param name="cancellationToken">The cancellation support token</param>
   /// <returns>The forecast retrieved from the external source</returns>
   Task<ServiceResult<WeatherForecast>> GetCurrentForecast(Location location, CancellationToken cancellationToken);
}