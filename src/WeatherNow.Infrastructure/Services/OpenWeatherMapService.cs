using System.Text.Json;
using WeatherNow.Application.Abstract.Services;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Geo;
using WeatherNow.Domain.Units;

namespace WeatherNow.Infrastructure.Services;

public class OpenWeatherMapService(
    HttpClient httpClient
    , OpenWeatherMapConfig config
    ) : IForecastService
{
    public async Task<ServiceResult<WeatherForecast>> GetCurrentForecast(Location location, CancellationToken cancellationToken)
    {
        (string key, string val)[] queryParams = [
            ("lat", location.Latitude.ToString())
            , ("lon", location.Longitude.ToString())
            , ("appid", config.ApiKey)
        ];

        var queryString = $"weather?{string.Join("&",  queryParams.Select(p => $"{p.key}={p.val}"))}";
        
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get
            , queryString);
        ServiceResult<WeatherForecast> serviceResult;
        try
        {
            var apiResponse = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

            if (apiResponse.IsSuccessStatusCode)
            {
                var json = await apiResponse.Content.ReadAsStringAsync(cancellationToken);
                var openWeatherMapForecast = JsonSerializer.Deserialize<OpenWeatherMapApiResponse>(json);
                if (openWeatherMapForecast != null)
                {
                    var forecast = GetWeatherForecastFromOpenMapResponse(openWeatherMapForecast);
                    serviceResult = new ServiceResult<WeatherForecast>
                    {
                        IsSuccessful = true,
                        Result = forecast,
                    };
                }
                else
                {
                    serviceResult = new ServiceResult<WeatherForecast>
                    {
                        IsSuccessful = false,
                        ErrorMessage = "Response from the api was not parsed successfully"
                    };
                }
            }
            else
            {
                serviceResult = new ServiceResult<WeatherForecast>
                {
                    IsSuccessful = false,
                    ErrorMessage = apiResponse.StatusCode.ToString()
                };
            }
        }
        catch (OperationCanceledException)
        {
            serviceResult = new ServiceResult<WeatherForecast>
            {
                IsSuccessful = false,
                IsCancelled = true
            };
        }
        catch (Exception ex)
        {
            serviceResult = new ServiceResult<WeatherForecast>
            {
                IsSuccessful = true,
                ErrorMessage = ex.Message
            };
        }

        return serviceResult;
        
    }

    public WeatherForecast GetWeatherForecastFromOpenMapResponse(OpenWeatherMapApiResponse apiResponse)
    {
        var mainWeather = apiResponse.Weather.FirstOrDefault();

        return new WeatherForecast
        {
            Description = mainWeather?.Description ?? string.Empty,
            Conditions = mainWeather?.Main ?? string.Empty,
            Temperature = new Temperature
            {
                Value = apiResponse.Main.Temp,
                Unit = TemperatureUnit.Kelvin
            },
            WindCondition = new WindCondition
            {
                Speed = apiResponse.Wind.Speed,
                Unit = SpeedUnit.MetersPerSecond
            },
            ForecastLocation = new Location
            {
                Latitude = apiResponse.Coord.Lat,
                Longitude = apiResponse.Coord.Lon
            }
        };
    }
}