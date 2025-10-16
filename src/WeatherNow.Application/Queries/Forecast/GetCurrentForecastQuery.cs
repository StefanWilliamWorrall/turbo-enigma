using Arta.Abstractions.Queries;
using Arta.Abstractions.Requests;
using Arta.Abstractions.Results;
using Microsoft.Extensions.Logging;
using WeatherNow.Application.Abstract.Services;
using WeatherNow.Application.Abstract.Util;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Geo;

namespace WeatherNow.Application.Queries.Forecast;

public record GetCurrentForecastQuery : IQuery<WeatherForecast>
{
    public required Location ForecastLocation { get; init; }
}

public class GetCurrentForecastQueryHandler(
    IForecastService forecastService
    , IForecastFormatter forecastFormatter
    , ILogger<GetCurrentForecastQueryHandler> logger) 
    : IQueryHandler<GetCurrentForecastQuery, WeatherForecast>
{
    public async Task<QueryResult<WeatherForecast>> HandleAsync(GetCurrentForecastQuery request, IRequestContext requestContext)
    {
        logger.LogDebug("{HandlerName}.{HandlerMethod} has been initiated", nameof(GetCurrentForecastQueryHandler), nameof(HandleAsync));
        QueryResult<WeatherForecast> queryResult;
        try
        {
            logger.LogDebug("Calling forecast service");
            var forecastResult = await forecastService.GetCurrentForecast(request.ForecastLocation, requestContext.CancellationToken);
            logger.LogDebug("Response from forecast service received. Processing response.");

            if (forecastResult is { IsSuccessful: true, Result: not null })
            {
                var formattedForecast = forecastFormatter.Format(forecastResult.Result);
                queryResult = new QuerySuccessResult<WeatherForecast>(formattedForecast, requestContext);
            }
            else if (forecastResult.IsCancelled)
            {
                queryResult = new QueryCancelledResult<WeatherForecast>(requestContext);
            }
            else
            {

                queryResult = new QueryFailureResult<WeatherForecast>(
                    forecastResult.ErrorMessage ?? "Error occured while accessing the forecast service"
                    , requestContext
                );
            }
        }
        catch (Exception exception)
        {
            logger.LogWarning("Exception thrown while getting current forecast service. Error: {Message}. {exception}",
                exception.Message, exception);
            queryResult = new QueryFailureResult<WeatherForecast>
            (
                exception.Message
                , requestContext
            );
        }
        finally
        {
            logger.LogDebug("{HandlerName}.{HandlerMethod} has been finished. Exiting method", nameof(GetCurrentForecastQueryHandler), nameof(HandleAsync));
        }

        return queryResult;
    }
}

