using System.Text.RegularExpressions;
using QRStudio.Application.Abstractions;
using QRStudio.Application.Commands;
using QRStudio.Application.Exceptions;
using QRStudio.Domain.Models;

namespace QRStudio.Application.Services;

public sealed partial class QrStudioService
{
    private const int MinimumPixelsPerModule = 3;
    private const int MaximumPixelsPerModule = 30;

    private readonly IQrCodeImageGenerator _imageGenerator;
    private readonly IQrHistoryRepository _historyRepository;
    private readonly IFileExportService _fileExportService;
    private readonly IClock _clock;

    public QrStudioService(
        IQrCodeImageGenerator imageGenerator,
        IQrHistoryRepository historyRepository,
        IFileExportService fileExportService,
        IClock clock)
    {
        _imageGenerator = imageGenerator;
        _historyRepository = historyRepository;
        _fileExportService = fileExportService;
        _clock = clock;
    }

    public async Task<QrGenerationResult> GenerateAsync(
        CreateQrCodeCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        ValidateDesign(command.Design);

        var payload = QrPayloadFormatter.Format(
            command.ContentType,
            command.PrimaryValue,
            command.SecondaryValue);

        var pngBytes = _imageGenerator.GeneratePng(
            payload,
            command.Design,
            command.ErrorCorrection);

        var item = new QrHistoryItem(
            Guid.NewGuid(),
            ResolveName(command.Name, command.ContentType.ToString()),
            command.ContentType,
            payload,
            command.Design,
            command.ErrorCorrection,
            _clock.UtcNow,
            null);

        await _historyRepository.UpsertAsync(item, cancellationToken);

        return new QrGenerationResult(item, pngBytes);
    }

    public Task<IReadOnlyList<QrHistoryItem>> GetHistoryAsync(
        CancellationToken cancellationToken = default)
    {
        return _historyRepository.GetAllAsync(cancellationToken);
    }

    public async Task<QrGenerationResult> ExportAsync(
        QrGenerationResult result,
        string destinationPath,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (string.IsNullOrWhiteSpace(destinationPath))
        {
            throw new QrStudioValidationException("Selecciona una ubicación válida para exportar.");
        }

        await _fileExportService.ExportAsync(
            destinationPath,
            result.PngBytes,
            cancellationToken);

        var updatedItem = result.Item with { LastExportedAtUtc = _clock.UtcNow };
        await _historyRepository.UpsertAsync(updatedItem, cancellationToken);

        return result with { Item = updatedItem };
    }

    public Task DeleteFromHistoryAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _historyRepository.DeleteAsync(id, cancellationToken);
    }

    public Task ClearHistoryAsync(CancellationToken cancellationToken = default)
    {
        return _historyRepository.ClearAsync(cancellationToken);
    }

    private static string ResolveName(string name, string fallback)
    {
        var trimmed = name.Trim();
        return string.IsNullOrWhiteSpace(trimmed) ? $"QR {fallback}" : trimmed;
    }

    private static void ValidateDesign(QrCodeDesign design)
    {
        ArgumentNullException.ThrowIfNull(design);

        if (!HexColorRegex().IsMatch(design.ForegroundHex)
            || !HexColorRegex().IsMatch(design.BackgroundHex))
        {
            throw new QrStudioValidationException("Los colores deben usar el formato hexadecimal #RRGGBB.");
        }

        if (design.PixelsPerModule is < MinimumPixelsPerModule or > MaximumPixelsPerModule)
        {
            throw new QrStudioValidationException(
                $"La escala debe estar entre {MinimumPixelsPerModule} y {MaximumPixelsPerModule}.");
        }

        if (string.Equals(
            design.ForegroundHex,
            design.BackgroundHex,
            StringComparison.OrdinalIgnoreCase))
        {
            throw new QrStudioValidationException(
                "El color del código y el fondo deben ser diferentes.");
        }
    }

    [GeneratedRegex("^#[0-9A-Fa-f]{6}$")]
    private static partial Regex HexColorRegex();
}
