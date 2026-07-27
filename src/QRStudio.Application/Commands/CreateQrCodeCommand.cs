using QRStudio.Domain.Enums;
using QRStudio.Domain.Models;

namespace QRStudio.Application.Commands;

public sealed record CreateQrCodeCommand(
    string Name,
    QrContentType ContentType,
    string PrimaryValue,
    string? SecondaryValue,
    QrCodeDesign Design,
    QrErrorCorrection ErrorCorrection);
