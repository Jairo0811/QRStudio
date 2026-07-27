using System.Text.Json;
using System.Text.Json.Serialization;
using QRStudio.Application.Abstractions;
using QRStudio.Domain.Models;

namespace QRStudio.Infrastructure.Persistence;

public sealed class JsonQrHistoryRepository : IQrHistoryRepository, IDisposable
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly string _filePath;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private bool _disposed;

    public JsonQrHistoryRepository(IAppDataPathProvider pathProvider)
    {
        ArgumentNullException.ThrowIfNull(pathProvider);
        _filePath = pathProvider.HistoryFilePath;
    }

    public async Task<IReadOnlyList<QrHistoryItem>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        await _gate.WaitAsync(cancellationToken);

        try
        {
            var items = await ReadUnsafeAsync(cancellationToken);
            return items
                .OrderByDescending(item => item.CreatedAtUtc)
                .ToArray();
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task UpsertAsync(
        QrHistoryItem item,
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(item);
        await _gate.WaitAsync(cancellationToken);

        try
        {
            var items = await ReadUnsafeAsync(cancellationToken);
            var existingIndex = items.FindIndex(existing => existing.Id == item.Id);

            if (existingIndex >= 0)
            {
                items[existingIndex] = item;
            }
            else
            {
                items.Add(item);
            }

            await WriteUnsafeAsync(items, cancellationToken);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        await _gate.WaitAsync(cancellationToken);

        try
        {
            var items = await ReadUnsafeAsync(cancellationToken);
            items.RemoveAll(item => item.Id == id);
            await WriteUnsafeAsync(items, cancellationToken);
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task ClearAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        await _gate.WaitAsync(cancellationToken);

        try
        {
            await WriteUnsafeAsync(Array.Empty<QrHistoryItem>(), cancellationToken);
        }
        finally
        {
            _gate.Release();
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _gate.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    private async Task<List<QrHistoryItem>> ReadUnsafeAsync(
        CancellationToken cancellationToken)
    {
        if (!File.Exists(_filePath))
        {
            return new List<QrHistoryItem>();
        }

        await using var stream = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<List<QrHistoryItem>>(
            stream,
            SerializerOptions,
            cancellationToken) ?? new List<QrHistoryItem>();
    }

    private async Task WriteUnsafeAsync(
        IReadOnlyCollection<QrHistoryItem> items,
        CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(_filePath)
            ?? throw new InvalidOperationException("No se pudo resolver la carpeta de datos.");

        Directory.CreateDirectory(directory);

        var temporaryPath = $"{_filePath}.{Guid.NewGuid():N}.tmp";

        try
        {
            await using (var stream = File.Create(temporaryPath))
            {
                await JsonSerializer.SerializeAsync(
                    stream,
                    items,
                    SerializerOptions,
                    cancellationToken);
            }

            File.Move(temporaryPath, _filePath, true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
            {
                File.Delete(temporaryPath);
            }
        }
    }
}
