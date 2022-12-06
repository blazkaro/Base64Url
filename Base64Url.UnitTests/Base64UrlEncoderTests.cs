using System.Security.Cryptography;

namespace Base64Url.UnitTests;

public class Base64UrlEncoderTests
{
    [Fact]
    public void Encoding_Returns_Valid_Base64Url()
    {
        var data = RandomNumberGenerator.GetBytes(17);

        var base64url = Base64UrlEncoder.Encode(data);

        Assert.Equal(Convert.ToBase64String(data)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('='), base64url);
    }

    // Below inline data are just random numbers 
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(7)]
    [InlineData(11)]
    [InlineData(18)]
    [InlineData(20)]
    [InlineData(42)]
    [InlineData(19)]
    [InlineData(57)]
    [InlineData(71)]
    public void Decoding_Returns_Valid_Data(int bytesCount)
    {
        var data = RandomNumberGenerator.GetBytes(bytesCount);

        var base64url = Base64UrlEncoder.Encode(data);

        Assert.Equal(data, Base64UrlEncoder.Decode(base64url));
    }

    [Fact]
    public void Decoding_Invalid_Base64Url_Throws()
    {
        Assert.Throws<ArgumentException>(() => Base64UrlEncoder.Decode("invalid=="));
        Assert.Throws<ArgumentException>(() => Base64UrlEncoder.Decode(""));
        Assert.Throws<ArgumentException>(() => Base64UrlEncoder.Decode(null));
    }

    [Fact]
    public void Validating_Valid_Base64Url_ReturnsTrue()
    {
        Assert.True(Base64UrlEncoder.Validate("validx", out _));
    }

    [Fact]
    public void Validating_Invalid_Base64Url_ReturnsFalse()
    {
        Assert.False(Base64UrlEncoder.Validate("invalid==", out _));
        Assert.False(Base64UrlEncoder.Validate("", out _));
        Assert.False(Base64UrlEncoder.Validate(null, out _));
    }
}
