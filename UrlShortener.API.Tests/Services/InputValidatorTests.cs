using FluentAssertions;
using UrlShortener.API.Services;

namespace UrlShortener.API.Tests.Services;

[TestFixture, Category("UnitTest")]
public class InputValidatorTests
{
    private InputValidator _sut = default!;

    [SetUp]
    public void Setup()
    {
        _sut = new InputValidator();
    }

    [Test]
    public void GivenAValidUrl_WhenCallingUrlIsValid_ReturnsTrue()
    {
        // Arrange
        var testUrl = "http://test.com/test";

        // Act
        var result = _sut.UrlIsValid(testUrl);

        // Assert
        result.Should().BeTrue();
    }

    [TestCase("qwerty")]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void GivenAnInvalidUrl_WhenCallingUrlIsValid_ReturnsFalse(string? url)
    {
        // Arrange / Act
        var result = _sut.UrlIsValid(url);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void GivenAValidShortCode_WhenCallingShortCodeIsValid_ReturnsTrue()
    {
        // Arrange
        var testShortCode = "test.shortcode";
        
        // Act
        var result = _sut.ShortCodeIsValid(testShortCode, testShortCode.Length);

        // Assert
        result.Should().BeTrue();
    }

    [TestCase("qwerty")]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("   ")]
    public void GivenAnInvalidShortCode_WhenCallingShortCodeIsValid_ReturnsFalse(string? shortCode)
    {
        // Arrange / Act
        var result = _sut.ShortCodeIsValid(shortCode, 7);

        // Assert
        result.Should().BeFalse();
    }
}