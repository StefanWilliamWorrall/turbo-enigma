namespace WeatherNow.Application.Abstract.Services;

public record ServiceResult
{
    public required bool IsSuccessful { get; init; }
    public bool IsCancelled { get; init; }
    public string? ErrorMessage;
    public object? ResultValue;

}

public record ServiceResult<T> : ServiceResult
{
    public T? Result;
    public new object? ResultValue => Result;
}