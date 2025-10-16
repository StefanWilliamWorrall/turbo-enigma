using Microsoft.Extensions.DependencyInjection;
using WeatherNow.Application.Abstract.Services;
using WeatherNow.Infrastructure.Services;

namespace WeatherNow.Infrastructure.Dependency;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient<IForecastService, OpenWeatherMapService>(OpenWeatherMapHttpClientExtensions.ConfigureHttpClientForOpenWeatherMapApi);
        serviceCollection.AddSingleton(OpenWeatherMapConfigDiExtensions.GetConfiguration);
        return serviceCollection;
    }
    
    
    
}