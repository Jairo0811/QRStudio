using System.IO;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRStudio.Application.Commands;
using QRStudio.Application.Exceptions;
using QRStudio.Application.Services;
using QRStudio.Domain.Enums;
using QRStudio.Domain.Models;
using QRStudio.Presentation.Services;

namespace QRStudio.Presentation.ViewModels;

public partial class CreateQrViewModel : ObservableObject
{
    private const string DefaultForeground = "#071225";
    private const string DefaultBackground = "#FFFFFF";

    private readonly QrStudioService _qrStudioService;
    private readonly IFileDialogService _fileDialogService;
    private QrGenerationResult? _currentResult;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GenerateCommand))]
    private string _primaryValue = string.Empty;

    [ObservableProperty]
    private string _secondaryValue = string.Empty;

    [ObservableProperty]
    private ContentTypeOption _selectedContentType;

    [ObservableProperty]
    private ErrorCorrectionOption _selectedErrorCorrection;

    [ObservableProperty]
    private string _foregroundHex = DefaultForeground;

    [ObservableProperty]
    private string _backgroundHex = DefaultBackground;

    [ObservableProperty]
    private int _pixelsPerModule = 12;

    [ObservableProperty]
    private bool _includeQuietZone = true;

    [ObservableProperty]
    private BitmapImage? _previewImage;

    [ObservableProperty]
    private bool _hasPreview;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GenerateCommand))]
    [NotifyCanExecuteChangedFor(nameof(ExportCommand))]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isError;

    [ObservableProperty]
    private string _statusMessage = "Configura el contenido y genera tu primer código QR.";

    public CreateQrViewModel(
        QrStudioService qrStudioService,
        IFileDialogService fileDialogService)
    {
        _qrStudioService = qrStudioService;
        _fileDialogService = fileDialogService;

        ContentTypes = new ContentTypeOption[]
        {
            new(QrContentType.Text, "Texto", "Contenido", "Escribe un mensaje o texto"),
            new(QrContentType.Website, "Sitio web", "Dirección web", "ejemplo.com"),
            new(QrContentType.Email, "Correo", "Correo electrónico", "nombre@dominio.com"),
            new(QrContentType.Phone, "Teléfono", "Número telefónico", "+1 809 555 0100"),
            new(
                QrContentType.Sms,
                "SMS",
                "Número telefónico",
                "+1 809 555 0100",
                true,
                "Mensaje",
                "Escribe el mensaje opcional")
        };

        ErrorCorrectionLevels = new ErrorCorrectionOption[]
        {
            new(QrErrorCorrection.Low, "Baja · 7%"),
            new(QrErrorCorrection.Medium, "Media · 15%"),
            new(QrErrorCorrection.Quartile, "Alta · 25%"),
            new(QrErrorCorrection.High, "Máxima · 30%")
        };

        _selectedContentType = ContentTypes[0];
        _selectedErrorCorrection = ErrorCorrectionLevels[1];
    }

    public IReadOnlyList<ContentTypeOption> ContentTypes { get; }

    public IReadOnlyList<ErrorCorrectionOption> ErrorCorrectionLevels { get; }

    public bool ShowSecondaryValue => SelectedContentType.HasSecondaryValue;

    partial void OnSelectedContentTypeChanged(ContentTypeOption value)
    {
        SecondaryValue = string.Empty;
        OnPropertyChanged(nameof(ShowSecondaryValue));
    }

    [RelayCommand(CanExecute = nameof(CanGenerate))]
    private async Task GenerateAsync()
    {
        IsBusy = true;
        IsError = false;
        StatusMessage = "Generando vista previa…";

        try
        {
            var command = new CreateQrCodeCommand(
                Name,
                SelectedContentType.Value,
                PrimaryValue,
                SecondaryValue,
                new QrCodeDesign(
                    ForegroundHex,
                    BackgroundHex,
                    PixelsPerModule,
                    IncludeQuietZone),
                SelectedErrorCorrection.Value);

            _currentResult = await _qrStudioService.GenerateAsync(command);
            PreviewImage = BitmapImageFactory.FromPngBytes(_currentResult.PngBytes);
            HasPreview = true;
            StatusMessage = "Código QR generado y añadido al historial.";
            ExportCommand.NotifyCanExecuteChanged();
        }
        catch (QrStudioValidationException exception)
        {
            SetError(exception.Message);
        }
        catch (Exception)
        {
            SetError("No se pudo generar el código QR. Inténtalo nuevamente.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanExport))]
    private async Task ExportAsync()
    {
        if (_currentResult is null)
        {
            return;
        }

        var destinationPath = _fileDialogService.ShowPngSaveDialog(
            $"{CreateSafeFileName(_currentResult.Item.Name)}.png");

        if (destinationPath is null)
        {
            return;
        }

        IsBusy = true;
        IsError = false;

        try
        {
            _currentResult = await _qrStudioService.ExportAsync(
                _currentResult,
                destinationPath);

            StatusMessage = $"Exportado correctamente en {destinationPath}.";
        }
        catch (Exception)
        {
            SetError("No se pudo exportar el archivo. Verifica la ubicación seleccionada.");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void ResetForm()
    {
        Name = string.Empty;
        PrimaryValue = string.Empty;
        SecondaryValue = string.Empty;
        SelectedContentType = ContentTypes[0];
        SelectedErrorCorrection = ErrorCorrectionLevels[1];
        ForegroundHex = DefaultForeground;
        BackgroundHex = DefaultBackground;
        PixelsPerModule = 12;
        IncludeQuietZone = true;
        PreviewImage = null;
        HasPreview = false;
        IsError = false;
        StatusMessage = "Configura el contenido y genera tu primer código QR.";
        _currentResult = null;
        ExportCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void ApplyForegroundPreset(string? color)
    {
        if (!string.IsNullOrWhiteSpace(color))
        {
            ForegroundHex = color;
        }
    }

    [RelayCommand]
    private void ApplyBackgroundPreset(string? color)
    {
        if (!string.IsNullOrWhiteSpace(color))
        {
            BackgroundHex = color;
        }
    }

    public void LoadFromHistory(QrHistoryItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        ResetForm();

        Name = item.Name;
        SelectedContentType = ContentTypes.First(option => option.Value == item.ContentType);
        SelectedErrorCorrection = ErrorCorrectionLevels.First(
            option => option.Value == item.ErrorCorrection);
        ForegroundHex = item.Design.ForegroundHex;
        BackgroundHex = item.Design.BackgroundHex;
        PixelsPerModule = item.Design.PixelsPerModule;
        IncludeQuietZone = item.Design.IncludeQuietZone;

        (PrimaryValue, SecondaryValue) = ExtractEditableValues(item);
        StatusMessage = "Configuración recuperada. Genera una nueva vista previa cuando estés listo.";
    }

    private bool CanGenerate()
    {
        return !IsBusy && !string.IsNullOrWhiteSpace(PrimaryValue);
    }

    private bool CanExport()
    {
        return !IsBusy && _currentResult is not null;
    }

    private void SetError(string message)
    {
        IsError = true;
        StatusMessage = message;
    }

    private static (string Primary, string Secondary) ExtractEditableValues(QrHistoryItem item)
    {
        return item.ContentType switch
        {
            QrContentType.Email => (item.Payload["mailto:".Length..], string.Empty),
            QrContentType.Phone => (item.Payload["tel:".Length..], string.Empty),
            QrContentType.Sms => ExtractSmsValues(item.Payload),
            _ => (item.Payload, string.Empty)
        };
    }

    private static (string Phone, string Message) ExtractSmsValues(string payload)
    {
        const string prefix = "sms:";
        const string bodySeparator = "?body=";

        var content = payload.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            ? payload[prefix.Length..]
            : payload;
        var separatorIndex = content.IndexOf(bodySeparator, StringComparison.Ordinal);

        if (separatorIndex < 0)
        {
            return (content, string.Empty);
        }

        return (
            content[..separatorIndex],
            Uri.UnescapeDataString(content[(separatorIndex + bodySeparator.Length)..]));
    }

    private static string CreateSafeFileName(string value)
    {
        var invalidCharacters = Path.GetInvalidFileNameChars();
        var safeCharacters = value.Select(
            character => invalidCharacters.Contains(character) ? '-' : character);
        var safeName = new string(safeCharacters.ToArray()).Trim();

        return string.IsNullOrWhiteSpace(safeName) ? "qr-studio" : safeName;
    }
}
