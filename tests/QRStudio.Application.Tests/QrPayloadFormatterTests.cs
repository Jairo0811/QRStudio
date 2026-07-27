using QRStudio.Application.Exceptions;
using QRStudio.Application.Services;
using QRStudio.Domain.Enums;

namespace QRStudio.Application.Tests;

public sealed class QrPayloadFormatterTests
{
    [Fact]
    public void Format_WhenWebsiteHasNoScheme_AddsHttps()
    {
        var result = QrPayloadFormatter.Format(
            QrContentType.Website,
            "jairo0811.github.io");

        Assert.Equal("https://jairo0811.github.io/", result);
    }

    [Theory]
    [InlineData(QrContentType.Email, "jairo@example.com", "mailto:jairo@example.com")]
    [InlineData(QrContentType.Phone, "+1 (809) 555-0100", "tel:+18095550100")]
    [InlineData(QrContentType.Text, "  Hola QR Studio  ", "Hola QR Studio")]
    public void Format_WhenValueIsValid_ReturnsExpectedPayload(
        QrContentType contentType,
        string value,
        string expected)
    {
        var result = QrPayloadFormatter.Format(contentType, value);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Format_WhenSmsHasMessage_EncodesBody()
    {
        var result = QrPayloadFormatter.Format(
            QrContentType.Sms,
            "809-555-0100",
            "Hola desde QR Studio");

        Assert.Equal(
            "sms:8095550100?body=Hola%20desde%20QR%20Studio",
            result);
    }

    [Theory]
    [InlineData(QrContentType.Website, "not a url")]
    [InlineData(QrContentType.Email, "correo-invalido")]
    [InlineData(QrContentType.Phone, "123")]
    public void Format_WhenValueIsInvalid_ThrowsValidationException(
        QrContentType contentType,
        string value)
    {
        Assert.Throws<QrStudioValidationException>(
            () => QrPayloadFormatter.Format(contentType, value));
    }
}
