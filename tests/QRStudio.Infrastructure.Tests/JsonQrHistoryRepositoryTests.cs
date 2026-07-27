using QRStudio.Domain.Enums;
using QRStudio.Domain.Models;
using QRStudio.Infrastructure.Persistence;

namespace QRStudio.Infrastructure.Tests;

public sealed class JsonQrHistoryRepositoryTests
{
    [Fact]
    public async Task UpsertAsync_PersistsItemAcrossRepositoryInstances()
    {
        var temporaryDirectory = Path.Combine(
            Path.GetTempPath(),
            $"qr-studio-tests-{Guid.NewGuid():N}");

        try
        {
            var pathProvider = new AppDataPathProvider(temporaryDirectory);
            var item = CreateHistoryItem();

            using (var writer = new JsonQrHistoryRepository(pathProvider))
            {
                await writer.UpsertAsync(item);
            }

            using var reader = new JsonQrHistoryRepository(pathProvider);
            var result = await reader.GetAllAsync();

            Assert.Single(result);
            Assert.Equal(item, result[0]);
        }
        finally
        {
            if (Directory.Exists(temporaryDirectory))
            {
                Directory.Delete(temporaryDirectory, true);
            }
        }
    }

    private static QrHistoryItem CreateHistoryItem()
    {
        return new QrHistoryItem(
            Guid.NewGuid(),
            "GitHub",
            QrContentType.Website,
            "https://github.com/Jairo0811",
            new QrCodeDesign("#071225", "#FFFFFF", 10, true),
            QrErrorCorrection.Medium,
            new DateTimeOffset(2026, 7, 27, 18, 0, 0, TimeSpan.Zero),
            null);
    }
}
