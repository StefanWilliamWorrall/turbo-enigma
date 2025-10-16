using System.Collections;
using Arta.Abstractions.Queries;
using Arta.Abstractions.Requests;
using Arta.Abstractions.Results;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherNow.Application.Abstract.Util;
using WeatherNow.Application.Dependency;
using WeatherNow.Application.Queries.Forecast;
using WeatherNow.Application.Util;
using WeatherNow.Domain.Forecast;
using WeatherNow.Infrastructure.Dependency;

namespace WeatherNow.Application.Tests;

[TestFixture]
public class DependencyInjectionFixture
{
    public static IEnumerable DIContainerTestCases
    {
        get
        {
            yield return new TestCaseData(typeof(ITemperatureConverter), typeof(TemperatureConverter))
                .SetName("TemperatureConverter is registered");
            yield return new TestCaseData(typeof(IWindSpeedConverter), typeof(WindSpeedConvertor))
                .SetName("WindSpeedConverter is registered");
            yield return new TestCaseData(typeof(IForecastFormatter), typeof(ForecastFormatter))
                .SetName("Forecast formatter is registered");
            yield return new TestCaseData(typeof(IRequestHandler<GetCurrentForecastQuery, QueryResult<WeatherForecast>>), typeof(GetCurrentForecastQueryHandler))
                .SetName("Query handler for GetCurrentForecastQuery is registered");
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
        services
            .AddApplicationServices()
            .AddInfrastructureServices()
            .AddLogging();
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