using QRStudio.Domain.Enums;
using QRStudio.Domain.Models;

namespace QRStudio.Application.Abstractions;

public interface IQrCodeImageGenerator
{
    byte[] GeneratePng(
        string payload,
        QrCodeDesign design,
        QrErrorCorrection errorCorrection);
}
