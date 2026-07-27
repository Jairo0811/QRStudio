namespace QRStudio.Infrastructure.Persistence;

public interface IAppDataPathProvider
{
    string HistoryFilePath { get; }
}
