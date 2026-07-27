using QRStudio.Domain.Models;

namespace QRStudio.Application.Abstractions;

public interface IQrHistoryRepository
{
    Task<IReadOnlyList<QrHistoryItem>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task UpsertAsync(
        QrHistoryItem item,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task ClearAsync(CancellationToken cancellationToken = default);
}
