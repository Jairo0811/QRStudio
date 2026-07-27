using QRStudio.Domain.Enums;

namespace QRStudio.Domain.Models;

public sealed record QrHistoryItem(
    Guid Id,
    string Name,
    QrContentType ContentType,
    string Payload,
    QrCodeDesign Design,
    QrErrorCorrection ErrorCorrection,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? LastExportedAtUtc);
