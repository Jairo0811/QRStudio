namespace QRStudio.Domain.Models;

public sealed record QrCodeDesign(
    string ForegroundHex,
    string BackgroundHex,
    int PixelsPerModule,
    bool IncludeQuietZone);
