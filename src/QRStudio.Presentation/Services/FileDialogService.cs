using Microsoft.Win32;

namespace QRStudio.Presentation.Services;

public sealed class FileDialogService : IFileDialogService
{
    public string? ShowPngSaveDialog(string suggestedFileName)
    {
        var dialog = new SaveFileDialog
        {
            AddExtension = true,
            DefaultExt = ".png",
            FileName = suggestedFileName,
            Filter = "Imagen PNG (*.png)|*.png",
            OverwritePrompt = true,
            Title = "Exportar código QR"
        };

        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }
}
