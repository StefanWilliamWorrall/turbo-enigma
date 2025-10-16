using Arta.Abstractions.Dispatch;
using Arta.Abstractions.Queries;
using Arta.Abstractions.Results;
using Microsoft.AspNetCore.Mvc;
using WeatherNow.Application.Queries.Forecast;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Geo;

namespace WeatherNow.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(IDispatcher dispatcher, ILogger<WeatherForecastController> logger) : ControllerBase
{
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get(
            double latitude
            , double longitude
            )
        {
            logger.LogDebug("Controller {ConntrollerName} and Controller Verb {ControllerVerb} have been called"
                 , nameof(WeatherForecastController)
                 , nameof(Get)
             );
            
            try
            {
                var query = new GetCurrentForecastQuery
                {
                    ForecastLocation = new Location
                    {
                        Latitude = latitude,
                        Longitude = longitude
                    }
                };

                logger.LogDebug("Sending Query to Application Logic");
                logger.LogTrace("Query will be run on location  Latitude={latitude}, Longitude={longitude}", latitude,
                    longitude);
                var queryResult = await dispatcher.SendQueryAsync<GetCurrentForecastQuery, WeatherForecast>(query);

                IActionResult apiResult;
                if (queryResult is QuerySuccessResult<WeatherForecast> successResult)
                {
                    apiResult = new OkObjectResult(successResult.Result);
                }
                else
                {
                    apiResult = new BadRequestObjectResult(queryResult);
                }
                
                logger.LogDebug("Query Results returned and processed. End of controller logic");
                return apiResult;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Error while getting weather forecast");
                return StatusCode(500, new {
                    message = exception.Message
                });
            }
        }
}