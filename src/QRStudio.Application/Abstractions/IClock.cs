namespace QRStudio.Application.Abstractions;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
