namespace QRStudio.Domain.Models;

public sealed record QrGenerationResult(
    QrHistoryItem Item,
    byte[] PngBytes);
