using QRStudio.Domain.Enums;

namespace QRStudio.Presentation.ViewModels;

public sealed record ContentTypeOption(
    QrContentType Value,
    string DisplayName,
    string PrimaryLabel,
    string PrimaryPlaceholder,
    bool HasSecondaryValue = false,
    string SecondaryLabel = "",
    string SecondaryPlaceholder = "");
