namespace QRStudio.Application.Abstractions;

public interface IFileExportService
{
    Task ExportAsync(
        string destinationPath,
        ReadOnlyMemory<byte> content,
        CancellationToken cancellationToken = default);
}
