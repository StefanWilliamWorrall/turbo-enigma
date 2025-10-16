using System.Reflection;
using Arta.Abstractions.Pipeline;
using Arta.Abstractions.Queries;
using Arta.Abstractions.Requests;
using Arta.Abstractions.Results;
using Arta.Core.Container;
using Arta.Core.Pipeline;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WeatherNow.Application.Abstract.Util;
using WeatherNow.Application.Pipeline;
using WeatherNow.Application.Queries.Forecast;
using WeatherNow.Application.Util;
using WeatherNow.Domain.Forecast;

namespace WeatherNow.Application.Dependency;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddArtaCore();
        services.AddScoped<IForecastFormatter, ForecastFormatter>();
        services.AddScoped<IRequestHandler<GetCurrentForecastQuery, QueryResult<WeatherForecast>>, GetCurrentForecastQueryHandler>();
        services.AddScoped<ITemperatureConverter, TemperatureConverter>();
        services.AddScoped<IWindSpeedConverter, WindSpeedConvertor>();
        services.AddScoped(typeof(IPipelineStage<,>), typeof(PipelineTelemetryStage<,>));
        services.AddScoped(typeof(IPipelineStage<,>), typeof(TransportLevelErrorSimulationPipeline<,>));
        services.AddScoped(typeof(IPipelineStage<,>), typeof(ValidationPipelineStage<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}