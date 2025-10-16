using FluentValidation;
using WeatherNow.Application.Validation;

namespace WeatherNow.Application.Queries.Forecast;

public class GetCurrentForecastQueryValidation : AbstractValidator<GetCurrentForecastQuery>
{
    public GetCurrentForecastQueryValidation()
    {
        RuleFor(x => x.ForecastLocation)
            .NotEmpty()
            .SetValidator(new LocationValidator());
    }
}