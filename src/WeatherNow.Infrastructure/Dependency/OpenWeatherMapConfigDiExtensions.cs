using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherNow.Infrastructure.Services;

namespace WeatherNow.Infrastructure.Dependency;

public static class OpenWeatherMapConfigDiExtensions
{
    public static OpenWeatherMapConfig GetConfiguration(IServiceProvider serviceProvider)
    {
        var appConfig = serviceProvider.GetRequiredService<IConfiguration>();
        var configSection = appConfig.GetSection("OpenWeatherMap");
        var openWeatherMapConfig = configSection.Get<OpenWeatherMapConfig>();
        return string.IsNullOrWhiteSpace(openWeatherMapConfig?.ApiKey) 
               || string.IsNullOrWhiteSpace(openWeatherMapConfig.BaseApiUrl)
            ? throw new InvalidOperationException("Missing configuration for the OpenWeatherMap API") 
            : openWeatherMapConfig;
    }
}