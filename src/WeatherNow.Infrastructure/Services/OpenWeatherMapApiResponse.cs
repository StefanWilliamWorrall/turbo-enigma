using System.Text.Json.Serialization;

namespace WeatherNow.Infrastructure.Services;

public class OpenWeatherMapApiResponse
{
    [JsonPropertyName("coord")]
    public Coord Coord { get; init; } = null!;

    [JsonPropertyName("weather")]
    public List<Weather> Weather { get; init; } = new();

    [JsonPropertyName("base")]
    public string Base { get; init; } = string.Empty;

    [JsonPropertyName("main")]
    public Main Main { get; init; } = null!;

    [JsonPropertyName("visibility")]
    public int Visibility { get; init; }

    [JsonPropertyName("wind")]
    public Wind Wind { get; init; } = null!;

    [JsonPropertyName("rain")]
    public Rain? Rain { get; init; }

    [JsonPropertyName("clouds")]
    public Clouds Clouds { get; init; } = null!;

    [JsonPropertyName("dt")]
    public long Dt { get; init; }

    [JsonPropertyName("sys")]
    public Sys Sys { get; init; } = null!;

    [JsonPropertyName("timezone")]
    public int Timezone { get; init; }

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("cod")]
    public int Cod { get; init; }
}

public record Coord
{
    [JsonPropertyName("lon")]
    public double Lon { get; init; }

    [JsonPropertyName("lat")]
    public double Lat { get; init; }
}

public record Weather
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("main")]
    public string Main { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; init; } = string.Empty;
}

public record Main
{
    [JsonPropertyName("temp")]
    public double Temp { get; init; }

    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; init; }

    [JsonPropertyName("temp_min")]
    public double TempMin { get; init; }

    [JsonPropertyName("temp_max")]
    public double TempMax { get; init; }

    [JsonPropertyName("pressure")]
    public int Pressure { get; init; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; init; }

    [JsonPropertyName("sea_level")]
    public int SeaLevel { get; init; }

    [JsonPropertyName("grnd_level")]
    public int GrndLevel { get; init; }
}

public record Wind
{
    [JsonPropertyName("speed")]
    public double Speed { get; init; }

    [JsonPropertyName("deg")]
    public int Deg { get; init; }

    [JsonPropertyName("gust")]
    public double Gust { get; init; }
}

public record Rain
{
    [JsonPropertyName("1h")]
    public double OneHour { get; init; }
}

public record Clouds
{
    [JsonPropertyName("all")]
    public int All { get; init; }
}

public record Sys
{
    [JsonPropertyName("type")]
    public int Type { get; init; }

    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("country")]
    public string Country { get; init; } = string.Empty;

    [JsonPropertyName("sunrise")]
    public long Sunrise { get; init; }

    [JsonPropertyName("sunset")]
    public long Sunset { get; init; }
}