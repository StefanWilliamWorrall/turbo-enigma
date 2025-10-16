using FluentAssertions;
using WeatherNow.Application.Util;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Units;

namespace WeatherNow.Application.Tests;

[TestFixture]
public class WindConverterFixture
{
    public static IEnumerable<TestCaseData> ConvertToKilometersPerHourCases
    {
        get
        {
            yield return new TestCaseData(
                new WindCondition { Speed = 10, Unit = SpeedUnit.MetersPerSecond },
                new WindCondition { Speed = 36, Unit = SpeedUnit.KilometersPerHour }
            ).SetName("ConvertToKilometersPerHour Should Convert m/s to km/h correctly");

            yield return new TestCaseData(
                new WindCondition { Speed = 5, Unit = SpeedUnit.MetersPerSecond },
                new WindCondition { Speed = 18, Unit = SpeedUnit.KilometersPerHour }
            ).SetName("ConvertToKilometersPerHour Should Convert m/s value correctly - second test case");
            
        }
    }
    
    [TestCaseSource(nameof(ConvertToKilometersPerHourCases))]
    public void ConvertToKilometersPerHour_ShouldConvertCorrectly(WindCondition input, WindCondition expected)
    {
        var sut = new WindSpeedConvertor();
        // Act
        var result = sut.ConvertToKilometersPerHour(input);

        // Assert
        result.Unit.Should().Be(expected.Unit);

        if (input.Unit == SpeedUnit.KilometersPerHour)
        {
            // Strict reference equality when already in km/h
            result.Should().BeSameAs(input);
        }
        else
        {
            // Numeric comparison when conversion happens
            result.Speed.Should().BeApproximately(expected.Speed, 0.0001);
            result.Should().NotBeSameAs(input);
        }
    }

    [Test]
    public void ConvertToKilometersPerHour_ShouldNotConvertForKilometersPerHour()
    {
        var conditions = new WindCondition
        {
            Speed = 20, Unit = SpeedUnit.KilometersPerHour
        };

        var sut = new WindSpeedConvertor();
        var result = sut.ConvertToKilometersPerHour(conditions);

        // Strict reference equality when already in km/h
        result.Should().BeSameAs(conditions);

    }
}