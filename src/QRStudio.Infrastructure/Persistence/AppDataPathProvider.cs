namespace QRStudio.Infrastructure.Persistence;

public sealed class AppDataPathProvider : IAppDataPathProvider
{
    private const string ApplicationDirectoryName = "QR Studio";
    private const string HistoryFileName = "history.json";

    public AppDataPathProvider()
        : this(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData))
    {
    }

    public AppDataPathProvider(string baseDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseDirectory);
        HistoryFilePath = Path.Combine(
            baseDirectory,
            ApplicationDirectoryName,
            HistoryFileName);
    }

    public string HistoryFilePath { get; }
}
