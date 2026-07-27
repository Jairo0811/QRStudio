namespace QRStudio.Application.Exceptions;

public sealed class QrStudioValidationException : Exception
{
    public QrStudioValidationException(string message)
        : base(message)
    {
    }
}
