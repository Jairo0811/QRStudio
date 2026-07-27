using System.Windows;

namespace QRStudio.Presentation.Services;

public sealed class UserDialogService : IUserDialogService
{
    public bool Confirm(string message, string title)
    {
        return MessageBox.Show(
            message,
            title,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    public void ShowError(string message, string title)
    {
        MessageBox.Show(
            message,
            title,
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}
