using FluentAssertions;
using NSubstitute;
using WeatherNow.Application.Abstract.Util;
using WeatherNow.Application.Tests.Helpers;
using WeatherNow.Application.Util;
using WeatherNow.Domain.Forecast;
using WeatherNow.Domain.Geo;
using WeatherNow.Domain.Units;

namespace WeatherNow.Application.Tests;

[TestFixture]
public class ForecastFormatterFixture
{
    
        [Test]
        public void Format_ShouldConvertTemperatureAndWindSpeed_AndBuildFormattedForecast()
        {
            var (fakeTempConverter, expectedTemp) = TemperatureConverterBuilder
                .Create()
                .WithDefaults()
                .BuildAndReturnExpected();

            var (fakeWindSpeedConverter, expectedWind) = WindSpeedConverterBuilder
                .Create()
                .WithDefaults()
                .BuildAndReturnExpected();

            var tempInput = new Temperature { Value = 50, Unit = TemperatureUnit.Fahrenheit };
            var windInput = new WindCondition { Speed = 5, Unit = SpeedUnit.MetersPerSecond };
            
            var forecast = new WeatherForecast
            {
                Temperature =tempInput,
                WindCondition = windInput,
                Conditions = "Clear",
                Description = "Sunny morning",
                ForecastLocation = new Location
                {
                     Latitude = 1,
                     Longitude = 1
                }
            };

            var sut = new ForecastFormatter(fakeTempConverter, fakeWindSpeedConverter);
            var actual = sut.Format(forecast);
            
            
            fakeTempConverter.Received(1).ConvertToCelsius(Arg.Is<Temperature>(t => ReferenceEquals(t, tempInput)));
            fakeWindSpeedConverter.Received(1).ConvertToKilometersPerHour(Arg.Is<WindCondition>(w => ReferenceEquals(w, windInput)));

            actual.Should().NotBeNull();
            actual.Temperature.Should().BeSameAs(expectedTemp);
            actual.WindCondition.Should().BeSameAs(expectedWind);
            actual.Conditions.Should().Be("Clear");
            actual.Description.Should().Be("Sunny morning");
            actual.Recommendation.Should().Contain("Don't forget to bring a hat.");
        }
        
        public static IEnumerable<TestCaseData> RecommendationCases
        {
            get
            {
                yield return new TestCaseData("Clear", 20, "Don't forget to bring a hat.")
                    .SetName("Clear sky -> Hat recommendation");
                yield return new TestCaseData("Rain", 18, "Don't forget the umbrella.")
                    .SetName("Raining -> Umbrella recommendation");
                yield return new TestCaseData("Snow", 5, "Don't forget to bring a coat.")
                    .SetName("Snowing and cold -> Coat recommendation");
                yield return new TestCaseData(string.Empty, 26, "It’s a great day for a swim.")
                    .SetName("Hot day -> Swimming recommendation");
                yield return new TestCaseData(string.Empty, 25, string.Empty)
                    .SetName("Warm Day -> Not Swimming recommendation [Swim Boundary Test]");
                yield return new TestCaseData("Drizzle", 10, "Don't forget to bring a coat.")
                    .SetName("Cold drizzle -> Coat recommendation");
                yield return new TestCaseData("Clear", 26, "Don't forget to bring a hat. It’s a great day for a swim.")
                    .SetName("Hot and Clear Day -> Swim and Hat recommendation");
                yield return new TestCaseData("Rain", 14, "Don't forget to bring a coat. Don't forget the umbrella.")
                    .SetName("Cold and Raining -> Umbrella and coat recommendation");
                yield return new TestCaseData("Rain", 15, "Don't forget the umbrella.")
                    .SetName("Raining -> Umbrella recommendation [Cold Boundary test]");
            }
        }

        private ForecastFormatter GetSutForUnitTest()
        {
            var fakeTempConverter = TemperatureConverterBuilder
                .Create()
                .WithDefaults()
                .Build();

            var fakeWindSpeedConverter = WindSpeedConverterBuilder
                .Create()
                .WithDefaults()
                .Build();

            var sut = new ForecastFormatter(fakeTempConverter, fakeWindSpeedConverter);
            return sut;
        }
        
        [Test]
        [TestCaseSource(nameof(RecommendationCases))]
        public void GetRecommendationForForecast_ShouldReturnExpectedRecommendations(
            string condition,
            double temperature,
            string expectedFragment)
        {
            
            var temp = new Temperature { Value = temperature, Unit = TemperatureUnit.Celsius };

            var sut = GetSutForUnitTest();
            
             var actual = sut.GetRecommendationForForecast(condition, temp);
           
             actual.Should().Be(expectedFragment);
        }
}