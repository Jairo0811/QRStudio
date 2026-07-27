using System.Globalization;
using System.Windows.Data;
using QRStudio.Domain.Enums;

namespace QRStudio.Presentation.Converters;

public sealed class ContentTypeDisplayConverter : IValueConverter
{
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        return value switch
        {
            QrContentType.Text => "Texto",
            QrContentType.Website => "Sitio web",
            QrContentType.Email => "Correo",
            QrContentType.Phone => "Teléfono",
            QrContentType.Sms => "SMS",
            _ => "QR"
        };
    }

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
