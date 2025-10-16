using Arta.Abstractions.Requests;
using NSubstitute;

namespace WeatherNow.Application.Tests.Helpers;

public class RequestContextBuilder
{
    private CancellationToken? _cancellationToken;
    private Dictionary<string,object>? _metadata;
    private Guid? _requestId;
    
    private RequestContextBuilder()
    {
    }


    public RequestContextBuilder WithDefaults()
    {
        _cancellationToken = CancellationToken.None;
        _metadata = Substitute.For<Dictionary<string, object>>();
        _requestId  = Guid.NewGuid();

        return this;
    }

    public RequestContextBuilder WithCancellationTokenCancelled()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();
       _cancellationToken = cts.Token;

        return this;
    }

    public IRequestContext Build()
    {
        var context = Substitute.For<IRequestContext>();
        context.CancellationToken.Returns(_cancellationToken ?? CancellationToken.None);
        context.Metadata.Returns(_metadata);
        context.RequestId.Returns(_requestId?.ToString("N"));
        
        return context;
    }

    public static RequestContextBuilder Create() => new();
}