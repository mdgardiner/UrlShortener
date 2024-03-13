using FluentAssertions;
using UrlShortener.API.Services;
using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Tests.Services;

[TestFixture, Category("UnitTest")]
public class UrlValidatorTests
{
    private UrlValidator _sut = default!;

    [SetUp]
    public void Setup()
    {
        _sut = new UrlValidator();
    }

    [Test]
    public void GivenAValidUrl_WhenCallingIsValid_ReturnsTrue()
    {
        // Arrange
        var testUrl = "http://test.com/test";

        // Act
        var result = _sut.IsValid(testUrl);

        // Assert
        result.Should().BeTrue();
    }

    [TestCase("qwerty")]
    [TestCase(null)]
    [TestCase("")]
    public void GivenAnInvalidUrl_WhenCallingIsValid_ReturnsFalse(string? url)
    {
        // Arrange
        var testUrl = url;
        
        // Act
        var result = _sut.IsValid(testUrl);

        // Assert
        result.Should().BeFalse();
    }
}