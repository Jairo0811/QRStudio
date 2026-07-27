using QRStudio.Domain.Enums;
using QRStudio.Domain.Models;
using QRStudio.Infrastructure.Generation;

namespace QRStudio.Infrastructure.Tests;

public sealed class QrCodeImageGeneratorTests
{
    [Fact]
    public void GeneratePng_WhenPayloadIsValid_ReturnsPngFile()
    {
        var generator = new QrCodeImageGenerator();
        var design = new QrCodeDesign("#071225", "#FFFFFF", 8, true);

        var result = generator.GeneratePng(
            "https://github.com/Jairo0811/GeneradorQR",
            design,
            QrErrorCorrection.Medium);

        Assert.True(result.Length > 100);
        Assert.Equal(
            new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 },
            result.Take(8).ToArray());
    }
}
