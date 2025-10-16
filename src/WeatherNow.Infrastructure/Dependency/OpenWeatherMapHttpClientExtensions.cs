using Microsoft.Extensions.DependencyInjection;
using WeatherNow.Infrastructure.Services;

namespace WeatherNow.Infrastructure.Dependency;

public static class OpenWeatherMapHttpClientExtensions
{
    public static void ConfigureHttpClientForOpenWeatherMapApi(
        IServiceProvider servicesProvider
        , HttpClient httpClient
        )
    {
        var config = servicesProvider.GetRequiredService<OpenWeatherMapConfig>();
        var baseUrl =  config.BaseApiUrl;
        if (Uri.TryCreate(baseUrl, UriKind.Absolute, out var baseUri))
        {
            httpClient.BaseAddress = baseUri;
        }
    }
}