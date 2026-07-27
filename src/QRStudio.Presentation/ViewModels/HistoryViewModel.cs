using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRStudio.Application.Services;
using QRStudio.Domain.Models;
using QRStudio.Presentation.Services;

namespace QRStudio.Presentation.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private readonly QrStudioService _qrStudioService;
    private readonly IUserDialogService _dialogService;
    private readonly IClipboardService _clipboardService;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteSelectedCommand))]
    [NotifyCanExecuteChangedFor(nameof(CopyPayloadCommand))]
    [NotifyCanExecuteChangedFor(nameof(RestoreCommand))]
    private QrHistoryItem? _selectedItem;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteSelectedCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearHistoryCommand))]
    private bool _isLoading;

    [ObservableProperty]
    private bool _hasItems;

    public HistoryViewModel(
        QrStudioService qrStudioService,
        IUserDialogService dialogService,
        IClipboardService clipboardService)
    {
        _qrStudioService = qrStudioService;
        _dialogService = dialogService;
        _clipboardService = clipboardService;
        Items.CollectionChanged += OnItemsChanged;
    }

    public ObservableCollection<QrHistoryItem> Items { get; } = new();

    public event Action<QrHistoryItem>? RestoreRequested;

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsLoading = true;

        try
        {
            var history = await _qrStudioService.GetHistoryAsync();
            Items.Clear();

            foreach (var item in history)
            {
                Items.Add(item);
            }

        }
        catch (Exception)
        {
            _dialogService.ShowError(
                "No se pudo cargar el historial local.",
                "QR Studio");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand(CanExecute = nameof(HasSelectedItem))]
    private void CopyPayload()
    {
        if (SelectedItem is null)
        {
            return;
        }

        _clipboardService.SetText(SelectedItem.Payload);
    }

    [RelayCommand(CanExecute = nameof(HasSelectedItem))]
    private void Restore()
    {
        if (SelectedItem is not null)
        {
            RestoreRequested?.Invoke(SelectedItem);
        }
    }

    [RelayCommand(CanExecute = nameof(CanDeleteSelected))]
    private async Task DeleteSelectedAsync()
    {
        if (SelectedItem is null
            || !_dialogService.Confirm(
                $"¿Quieres eliminar “{SelectedItem.Name}” del historial?",
                "Eliminar elemento"))
        {
            return;
        }

        try
        {
            var itemToDelete = SelectedItem;
            await _qrStudioService.DeleteFromHistoryAsync(itemToDelete.Id);
            Items.Remove(itemToDelete);
            SelectedItem = null;
        }
        catch (Exception)
        {
            _dialogService.ShowError(
                "No se pudo eliminar el elemento del historial.",
                "QR Studio");
        }
    }

    [RelayCommand(CanExecute = nameof(CanClearHistory))]
    private async Task ClearHistoryAsync()
    {
        if (!_dialogService.Confirm(
            "Esta acción eliminará todo el historial local. ¿Deseas continuar?",
            "Vaciar historial"))
        {
            return;
        }

        try
        {
            await _qrStudioService.ClearHistoryAsync();
            Items.Clear();
            SelectedItem = null;
        }
        catch (Exception)
        {
            _dialogService.ShowError(
                "No se pudo vaciar el historial local.",
                "QR Studio");
        }
    }

    private bool HasSelectedItem()
    {
        return SelectedItem is not null;
    }

    private bool CanDeleteSelected()
    {
        return !IsLoading && SelectedItem is not null;
    }

    private bool CanClearHistory()
    {
        return !IsLoading && Items.Count > 0;
    }

    private void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        HasItems = Items.Count > 0;
        ClearHistoryCommand.NotifyCanExecuteChanged();
    }
}
