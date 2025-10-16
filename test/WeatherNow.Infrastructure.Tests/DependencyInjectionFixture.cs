using System.Collections;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherNow.Application.Abstract.Services;
using WeatherNow.Infrastructure.Dependency;
using WeatherNow.Infrastructure.Services;

namespace WeatherNow.Infrastructure.Tests;

[TestFixture]
public class DependencyInjectionFixture
{
    
    public static IEnumerable DIContainerTestCases
    {
        get
        {
            yield return new TestCaseData(typeof(IForecastService), typeof(OpenWeatherMapService));
        }
    }
    
    private IServiceProvider _serviceProvider;
    
    [OneTimeSetUp]
    public void FixtureSetup()
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
        services.AddInfrastructureServices();
        _serviceProvider = services.BuildServiceProvider();
    }
    

    [Test]
    [TestCaseSource(nameof(DIContainerTestCases))]
    public void AddApplicationServices_ShouldRegisterTheExpectedConcreteObjects(
        Type contractType
        ,Type expectedImplementation)
    {
        
        var actual = _serviceProvider.GetService(contractType);
        actual.Should().BeOfType(expectedImplementation);
        
    }
    
    
}