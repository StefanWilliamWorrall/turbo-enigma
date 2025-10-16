using System.Diagnostics;
using Arta.Abstractions.Pipeline;
using Arta.Abstractions.Requests;
using Microsoft.Extensions.Logging;

namespace WeatherNow.Application.Pipeline;

public class PipelineTelemetryStage<TRequest, TResult>(ILogger<PipelineTelemetryStage<TRequest, TResult>> logger) 
    : IPipelineStage<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    public async Task<TResult> ProcessAsync(
        TRequest request
        , PipelineStageProcessorDelegate<TResult> nextPipelineStageProcessor,
        IRequestContext requestContext)
    {
        var pipelineStartExecutionTime = DateTime.UtcNow;
        var pipelineTimer = Stopwatch.StartNew();
        logger.LogDebug("Pipeline Started Execution at {PipelineExecutionStartTime}", pipelineStartExecutionTime);

        try
        {
            return await nextPipelineStageProcessor();
        }
        finally
        {
            var pipelineStopExecutionTime = DateTime.UtcNow;
            pipelineTimer.Stop();

            requestContext.Metadata.Add("PipelineStartExecutionTime", pipelineStartExecutionTime);
            requestContext.Metadata.Add("PipelineStopExecutionTime", pipelineStopExecutionTime);
            requestContext.Metadata.Add("PipelineExecutionTime", pipelineTimer.Elapsed);

            logger.LogDebug(
                "Pipeline Exited Execution at {PipelineExecutionEnd}. Pipeline Execution Duration {PipelineExecutionDuration}"
                , pipelineStopExecutionTime
                , pipelineTimer.Elapsed);
        }
    }
}