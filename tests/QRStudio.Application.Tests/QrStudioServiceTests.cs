using QRStudio.Application.Abstractions;
using QRStudio.Application.Commands;
using QRStudio.Application.Services;
using QRStudio.Domain.Enums;
using QRStudio.Domain.Models;

namespace QRStudio.Application.Tests;

public sealed class QrStudioServiceTests
{
    [Fact]
    public async Task GenerateAsync_WhenCommandIsValid_GeneratesAndPersistsItem()
    {
        var repository = new InMemoryHistoryRepository();
        var imageGenerator = new StubImageGenerator();
        var service = CreateService(repository, imageGenerator);
        var command = new CreateQrCodeCommand(
            "Mi portafolio",
            QrContentType.Website,
            "jairo0811.github.io",
            null,
            new QrCodeDesign("#071225", "#FFFFFF", 12, true),
            QrErrorCorrection.Medium);

        var result = await service.GenerateAsync(command);

        Assert.Equal("https://jairo0811.github.io/", result.Item.Payload);
        Assert.Equal(new byte[] { 137, 80, 78, 71 }, result.PngBytes);
        Assert.Single(repository.Items);
        Assert.Equal(result.Item, repository.Items[0]);
        Assert.Equal("https://jairo0811.github.io/", imageGenerator.LastPayload);
    }

    private static QrStudioService CreateService(
        InMemoryHistoryRepository repository,
        StubImageGenerator imageGenerator)
    {
        return new QrStudioService(
            imageGenerator,
            repository,
            new StubFileExportService(),
            new FixedClock(new DateTimeOffset(2026, 7, 27, 18, 0, 0, TimeSpan.Zero)));
    }

    private sealed class StubImageGenerator : IQrCodeImageGenerator
    {
        public string? LastPayload { get; private set; }

        public byte[] GeneratePng(
            string payload,
            QrCodeDesign design,
            QrErrorCorrection errorCorrection)
        {
            LastPayload = payload;
            return new byte[] { 137, 80, 78, 71 };
        }
    }

    private sealed class InMemoryHistoryRepository : IQrHistoryRepository
    {
        public List<QrHistoryItem> Items { get; } = new();

        public Task<IReadOnlyList<QrHistoryItem>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyList<QrHistoryItem>>(Items);
        }

        public Task UpsertAsync(
            QrHistoryItem item,
            CancellationToken cancellationToken = default)
        {
            Items.RemoveAll(existing => existing.Id == item.Id);
            Items.Add(item);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            Items.RemoveAll(item => item.Id == id);
            return Task.CompletedTask;
        }

        public Task ClearAsync(CancellationToken cancellationToken = default)
        {
            Items.Clear();
            return Task.CompletedTask;
        }
    }

    private sealed class StubFileExportService : IFileExportService
    {
        public Task ExportAsync(
            string destinationPath,
            ReadOnlyMemory<byte> content,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FixedClock : IClock
    {
        public FixedClock(DateTimeOffset utcNow)
        {
            UtcNow = utcNow;
        }

        public DateTimeOffset UtcNow { get; }
    }
}
