using QRStudio.Application.Abstractions;

namespace QRStudio.Infrastructure.System;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
