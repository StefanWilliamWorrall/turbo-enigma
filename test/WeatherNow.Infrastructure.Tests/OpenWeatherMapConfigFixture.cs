using System.Collections;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherNow.Infrastructure.Dependency;

namespace WeatherNow.Infrastructure.Tests;

[TestFixture]
public class OpenWeatherMapConfigFixture
{
    [Test]
    public void GetConfiguration_ShouldReturnConfigurationObjectWhenPresent()
    {
        var testConfig = new Dictionary<string, string?>
        {
            ["OpenWeatherMap:ApiKey"] = "test-api-key",
            ["OpenWeatherMap:BaseApiUrl"] = "test-api-url",
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfig)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        var provider = services.BuildServiceProvider();
        
        var result = OpenWeatherMapConfigDiExtensions.GetConfiguration(provider);
        
        result.Should().NotBeNull();
        result.ApiKey.Should().Be("test-api-key");
    }

    public static IEnumerable ConfigExceptionTestCases
    {
        get
        {
            yield return new TestCaseData(new Dictionary<string,string?>())
                .SetName("No OpenWeatherMap config section loaded found");
            yield return new TestCaseData(new Dictionary<string, string?>
                {
                    ["OpenWeatherMap:ApiKey"] = null
                })
                .SetName("API Key missing from OpenWeatherMap section");

        }
    }
    
    [Test]
    [TestCaseSource(nameof(ConfigExceptionTestCases))]
    public void GetConfiguration_ShouldThrowExceptionWhenConfigIsMissing(Dictionary<string, string?> testConfig)
    {
        
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfig)
            .Build();
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        var provider = services.BuildServiceProvider();

        
        var actual = Assert.Throws<InvalidOperationException>(() => OpenWeatherMapConfigDiExtensions.GetConfiguration(provider));
        
        actual.Message.Should().Be("Missing configuration for the OpenWeatherMap API");
    }
}