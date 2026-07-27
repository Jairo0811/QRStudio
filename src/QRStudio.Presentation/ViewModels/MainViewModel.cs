using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace QRStudio.Presentation.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly CreateQrViewModel _createQrViewModel;
    private readonly HistoryViewModel _historyViewModel;
    private readonly AboutViewModel _aboutViewModel;

    [ObservableProperty]
    private object _currentPage;

    public MainViewModel(
        CreateQrViewModel createQrViewModel,
        HistoryViewModel historyViewModel,
        AboutViewModel aboutViewModel)
    {
        _createQrViewModel = createQrViewModel;
        _historyViewModel = historyViewModel;
        _aboutViewModel = aboutViewModel;
        _currentPage = _createQrViewModel;

        _historyViewModel.RestoreRequested += RestoreFromHistory;
    }

    public string Version => _aboutViewModel.Version;

    [RelayCommand]
    private async Task NavigateAsync(string? destination)
    {
        switch (destination)
        {
            case "create":
                CurrentPage = _createQrViewModel;
                break;

            case "history":
                await _historyViewModel.LoadAsync();
                CurrentPage = _historyViewModel;
                break;

            case "about":
                CurrentPage = _aboutViewModel;
                break;
        }
    }

    private void RestoreFromHistory(QRStudio.Domain.Models.QrHistoryItem item)
    {
        _createQrViewModel.LoadFromHistory(item);
        CurrentPage = _createQrViewModel;
    }
}
