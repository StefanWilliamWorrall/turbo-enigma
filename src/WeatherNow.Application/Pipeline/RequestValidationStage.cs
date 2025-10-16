using Arta.Abstractions.Pipeline;
using Arta.Abstractions.Requests;
using FluentValidation;

namespace WeatherNow.Application.Pipeline;

public class ValidationPipelineStage<TRequest, TResult>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineStage<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    public async Task<TResult> ProcessAsync(
        TRequest request,
        PipelineStageProcessorDelegate<TResult> next,
        IRequestContext requestContext)
    {
        if (validators.Any())
        {
            var validationTasks = validators.Select(v => v.ValidateAsync(request));
            var results = await Task.WhenAll(validationTasks);

            var failures = results
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();

            if (failures.Count != 0)
                throw new ValidationException(failures);
        }

        return await next();
    }
}