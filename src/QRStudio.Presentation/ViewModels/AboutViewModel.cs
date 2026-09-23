namespace QRStudio.Presentation.ViewModels;

public sealed class AboutViewModel
{
    public string Version { get; } =
        typeof(AboutViewModel).Assembly.GetName().Version?.ToString(3) ?? "1.0.0";
}
