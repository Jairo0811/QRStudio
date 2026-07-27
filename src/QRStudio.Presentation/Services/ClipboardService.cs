using System.Windows;

namespace QRStudio.Presentation.Services;

public sealed class ClipboardService : IClipboardService
{
    public void SetText(string text)
    {
        Clipboard.SetText(text);
    }
}
