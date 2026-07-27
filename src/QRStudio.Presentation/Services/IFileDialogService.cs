namespace QRStudio.Presentation.Services;

public interface IFileDialogService
{
    string? ShowPngSaveDialog(string suggestedFileName);
}
