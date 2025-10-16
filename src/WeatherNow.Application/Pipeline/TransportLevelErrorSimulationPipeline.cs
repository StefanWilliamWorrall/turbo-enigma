using Arta.Abstractions.Pipeline;
using Arta.Abstractions.Requests;
using Microsoft.Extensions.Logging;

namespace WeatherNow.Application.Pipeline;

public class TransportLevelErrorSimulationPipeline<TRequest, TResult>(ILogger<PipelineTelemetryStage<TRequest, TResult>> logger) 
    : IPipelineStage<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private static int _apiCallCount = 0;
    
    public async Task<TResult> ProcessAsync(TRequest request, PipelineStageProcessorDelegate<TResult> nextPipelineStageProcessor,
        IRequestContext requestContext)
    {
        logger.LogDebug("Checking to see if we have to simulate the transport level error.");
        var currentCount = Interlocked.Increment(ref _apiCallCount);

        if (currentCount % 5 == 0)
        {
            logger.LogDebug("Throwing test exception.");
            throw new InvalidOperationException("The API had communication issues");
        }
        
        return await nextPipelineStageProcessor();
    }
}