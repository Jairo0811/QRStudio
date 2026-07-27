using System.Globalization;
using QRStudio.Application.Abstractions;
using QRStudio.Domain.Enums;
using QRStudio.Domain.Models;
using QRCoder;

namespace QRStudio.Infrastructure.Generation;

public sealed class QrCodeImageGenerator : IQrCodeImageGenerator
{
    public byte[] GeneratePng(
        string payload,
        QrCodeDesign design,
        QrErrorCorrection errorCorrection)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(payload);
        ArgumentNullException.ThrowIfNull(design);

        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(payload, Map(errorCorrection));
        using var qrCode = new PngByteQRCode(data);

        return qrCode.GetGraphic(
            design.PixelsPerModule,
            ParseRgba(design.ForegroundHex),
            ParseRgba(design.BackgroundHex),
            design.IncludeQuietZone);
    }

    private static QRCodeGenerator.ECCLevel Map(QrErrorCorrection level)
    {
        return level switch
        {
            QrErrorCorrection.Low => QRCodeGenerator.ECCLevel.L,
            QrErrorCorrection.Medium => QRCodeGenerator.ECCLevel.M,
            QrErrorCorrection.Quartile => QRCodeGenerator.ECCLevel.Q,
            QrErrorCorrection.High => QRCodeGenerator.ECCLevel.H,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }

    private static byte[] ParseRgba(string hexColor)
    {
        return new byte[]
        {
            byte.Parse(hexColor.AsSpan(1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture),
            byte.Parse(hexColor.AsSpan(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture),
            byte.Parse(hexColor.AsSpan(5, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture),
            byte.MaxValue
        };
    }
}
