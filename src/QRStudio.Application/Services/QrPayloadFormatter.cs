using System.Net.Mail;
using System.Text.RegularExpressions;
using QRStudio.Application.Exceptions;
using QRStudio.Domain.Enums;

namespace QRStudio.Application.Services;

public static partial class QrPayloadFormatter
{
    public static string Format(
        QrContentType contentType,
        string primaryValue,
        string? secondaryValue = null)
    {
        var value = primaryValue.Trim();

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new QrStudioValidationException("Escribe el contenido que tendrá el código QR.");
        }

        return contentType switch
        {
            QrContentType.Text => value,
            QrContentType.Website => FormatWebsite(value),
            QrContentType.Email => FormatEmail(value),
            QrContentType.Phone => $"tel:{NormalizePhone(value)}",
            QrContentType.Sms => FormatSms(value, secondaryValue),
            _ => throw new QrStudioValidationException("El tipo de contenido no es compatible.")
        };
    }

    private static string FormatWebsite(string value)
    {
        var normalized = value.Contains("://", StringComparison.Ordinal)
            ? value
            : $"https://{value}";

        if (!Uri.TryCreate(normalized, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            || string.IsNullOrWhiteSpace(uri.Host))
        {
            throw new QrStudioValidationException("Escribe una dirección web válida.");
        }

        return uri.AbsoluteUri;
    }

    private static string FormatEmail(string value)
    {
        try
        {
            var address = new MailAddress(value);

            if (!string.Equals(address.Address, value, StringComparison.OrdinalIgnoreCase))
            {
                throw new FormatException();
            }

            return $"mailto:{address.Address}";
        }
        catch (FormatException)
        {
            throw new QrStudioValidationException("Escribe una dirección de correo válida.");
        }
    }

    private static string FormatSms(string value, string? message)
    {
        var phone = NormalizePhone(value);
        var body = message?.Trim();

        return string.IsNullOrWhiteSpace(body)
            ? $"sms:{phone}"
            : $"sms:{phone}?body={Uri.EscapeDataString(body)}";
    }

    private static string NormalizePhone(string value)
    {
        var normalized = PhoneCharactersRegex().Replace(value, string.Empty);

        if (normalized.Length < 7
            || (normalized.Count(character => character == '+') > 1)
            || (normalized.Contains('+') && !normalized.StartsWith('+')))
        {
            throw new QrStudioValidationException("Escribe un número telefónico válido.");
        }

        return normalized;
    }

    [GeneratedRegex(@"[^\d+]")]
    private static partial Regex PhoneCharactersRegex();
}
