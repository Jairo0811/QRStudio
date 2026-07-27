using QRStudio.Domain.Enums;

namespace QRStudio.Presentation.ViewModels;

public sealed record ErrorCorrectionOption(
    QrErrorCorrection Value,
    string DisplayName);
