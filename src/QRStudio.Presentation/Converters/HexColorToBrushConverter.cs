using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace QRStudio.Presentation.Converters;

public sealed class HexColorToBrushConverter : IValueConverter
{
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (value is not string hex)
        {
            return Brushes.Transparent;
        }

        try
        {
            return new BrushConverter().ConvertFromString(hex) as Brush
                ?? Brushes.Transparent;
        }
        catch (FormatException)
        {
            return Brushes.Transparent;
        }
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
