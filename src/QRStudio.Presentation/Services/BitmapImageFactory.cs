using System.IO;
using System.Windows.Media.Imaging;

namespace QRStudio.Presentation.Services;

public static class BitmapImageFactory
{
    public static BitmapImage FromPngBytes(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes);

        using var stream = new MemoryStream(bytes);
        var image = new BitmapImage();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = stream;
        image.EndInit();
        image.Freeze();
        return image;
    }
}
