using System.Collections;
using FluentAssertions;
using WeatherNow.Application.Util;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Units;

namespace WeatherNow.Application.Tests;

[TestFixture]
public class TemperatureConverterTests
{

    [Test]
    public void ConvertToCelsius_FromCelsius_ShouldReturnsSameObject()
    {
        var source = new Temperature { Value = 25, Unit = TemperatureUnit.Celsius };
        var sut = new TemperatureConverter();
        var actual = sut.ConvertToCelsius(source);

        actual.Should().BeSameAs(source);

    }

    public static IEnumerable CToFTestCases
    {
        get
        {
            yield return new TestCaseData(32, 0)
                .SetName("ConvertToCelsius Should Not Convert Values When Fahrenheit Are Received [32F -> 0C]");
            yield return new TestCaseData(212, 100)
                .SetName("ConvertToCelsius Should Not Convert Values When Fahrenheit Are Received [212F -> 100C]");
            yield return new TestCaseData(-40, -40)
                .SetName("ConvertToCelsius Should Not Convert Values When Fahrenheit Are Received [-40F -> -40C]");
        }
    }

    [Test]
    [TestCaseSource(nameof(CToFTestCases))]
    public void ConvertToCelsius_FromFahrenheit_ReturnsExpectedValue(double input, double expected)
    {
        var source = new Temperature { Value = input, Unit = TemperatureUnit.Fahrenheit };
        var sut = new TemperatureConverter();
        var result = sut.ConvertToCelsius(source);

        result.Value.Should().Be(expected);
        result.Unit.Should().Be(TemperatureUnit.Celsius);
    }

    public static IEnumerable KToCTestCases
    {
        get
        {
            yield return new TestCaseData(273.15, 0)
                .SetName("ConvertToCelsius Should Not Convert Values When Kelvin Are Received [273.15K -> 0C]");
            yield return new TestCaseData(373.15, 100)
                .SetName("ConvertToCelsius Should Not Convert Values When Kelvin Are Received [373.15K -> 100C]");
            yield return new TestCaseData(233.15, -40)
                .SetName("ConvertToCelsius Should Not Convert Values When Kelvin Are Received [373.15K -> -40C]");
        }
    }
    
    [Test]
    [TestCaseSource(nameof(KToCTestCases))]
    public void ConvertToCelsius_FromKelvin_ReturnsExpectedValue(double input, double expected)
    {
        var source = new Temperature { Value = input, Unit = TemperatureUnit.Kelvin };
        var sut = new TemperatureConverter();
        var result = sut.ConvertToCelsius(source);

        result.Value.Should().BeApproximately(expected, 1e-5);
        result.Unit.Should().Be(TemperatureUnit.Celsius);
    }

    [Test]
    public void ConvertToCelsius_WithInvalidUnit_ThrowsException()
    {
        var invalidUnit = (TemperatureUnit)999;
        var source = new Temperature { Value = 100, Unit = invalidUnit };
        var sut = new TemperatureConverter();
        Assert.Throws<ArgumentOutOfRangeException>(() => sut.ConvertToCelsius(source));
    }
}