using System.Linq.Expressions;
using FluentAssertions;

using Microsoft.Extensions.Logging;
using Moq;
using UrlShortener.API.Entities;
using UrlShortener.API.Repositories.Interfaces;
using UrlShortener.API.Services;
using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Tests.Services;

[TestFixture, Category("UnitTest")]
public class ShortenedUrlService_CreateShortenedUrl_Tests
{
    private const string ShortCodePrefixUrl = "http://short.com"; 
    
    private Mock<IUrlValidator> _urlValidatorMock = default!;
    private Mock<IRepository<ShortenedUrl>> _shortenedUrlRepositoryMock = default!;
    private Mock<IShortCodeUrlPrefixer> _shortCodeUrlPrefixerMock = default!;
    private Mock<IShortCodeGenerator> _shortCodeGeneratorMock = default!;
    private Mock<ILogger<ShortenedUrlService>> _loggerMock = default!;
    
    private ShortenedUrlService _sut = default!;

    [SetUp]
    public void Setup()
    {
        _urlValidatorMock = new Mock<IUrlValidator>();
        _urlValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

        _shortenedUrlRepositoryMock = new Mock<IRepository<ShortenedUrl>>();

        _shortCodeUrlPrefixerMock = new Mock<IShortCodeUrlPrefixer>();
        _shortCodeUrlPrefixerMock.Setup(x => 
                x.GetPrefixedUrl(It.IsAny<string>()))
            .Returns<string>(shortcode => new Uri(string.Join('/', ShortCodePrefixUrl, shortcode)));

        _shortCodeGeneratorMock = new Mock<IShortCodeGenerator>();
        
        _loggerMock = new Mock<ILogger<ShortenedUrlService>>();

        _sut = new ShortenedUrlService(
            _urlValidatorMock.Object, 
            _shortenedUrlRepositoryMock.Object,
            _shortCodeUrlPrefixerMock.Object,
            _shortCodeGeneratorMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public void GivenAUrl_WhenCallingCreateShortenedUrl_ThenUrlIsValidated()
    {
        // Arrange
        var testUrl = "test.url";
        
        // Act
        _sut.GetShortenedUrl(testUrl);

        // Assert
        _urlValidatorMock.Verify(x => x.IsValid(testUrl), Times.Once);
    }

    [Test]
    public void GivenAUrlAndValidationFails_WhenCallingCreateShortenedUrl_ThenArgumentExceptionIsThrown()
    {
        // Arrange
        var testUrl = "test.url";
        _urlValidatorMock.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);

        // Act / Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            _sut.GetShortenedUrl(testUrl));

        exception?.Message.Should().Be("test.url is not a valid url");
    }

    [Test]
    public void GivenAUrlThatHasAlreadyBeenShortened_WhenCallingCreateShortenedUrl_ThenReturnsExistingShortUrl()
    {
        // Arrange
        var testLongUrl = "test.long.url";
        var testShortCode = "test.short.code";

        _shortenedUrlRepositoryMock.Setup(x =>
                x.SearchForSingleOrDefault(
                    It.IsAny<Expression<Func<ShortenedUrl, bool>>>()))
            .Returns(new ShortenedUrl{ LongUrl = testLongUrl, ShortCode = testShortCode });

        // Act
        var result = _sut.GetShortenedUrl(testLongUrl);

        // Assert
        _shortenedUrlRepositoryMock.Verify(x => 
            x.SearchForSingleOrDefault(It.IsAny<Expression<Func<ShortenedUrl, bool>>>()), Times.Once);
        _shortCodeUrlPrefixerMock.Verify(x => x.GetPrefixedUrl(testShortCode), Times.Once);
        result.Should().Be($"{ShortCodePrefixUrl}/{testShortCode}");
    }

    [Test]
    public void
        GivenAUrlThatHasNotBeenShortened_WhenCallingCreateShortenedUrl_ThenShortCodeIsCreatedSavedAndUriReturned()
    {
        // Arrange
        var testLongUrl = "test.long.url";
        var testShortCode = "test.short.code";

        _shortCodeGeneratorMock.Setup(x => x.GenerateShortCode(It.IsAny<int>()))
            .Returns(testShortCode);
        
        // Act
        var result = _sut.GetShortenedUrl(testLongUrl);

        // Assert
        _shortCodeGeneratorMock.Verify(x => x.GenerateShortCode(7), Times.Once);
        _shortCodeUrlPrefixerMock.Verify(x => x.GetPrefixedUrl(testShortCode), Times.Once);
        _shortenedUrlRepositoryMock.Verify(x => x.Insert(
            It.Is<ShortenedUrl>(y => 
                y.LongUrl == testLongUrl && 
                y.ShortCode == testShortCode)),
            Times.Once);
        _shortenedUrlRepositoryMock.Verify(x => x.Commit(), Times.Once);
        result.Should().Be($"{ShortCodePrefixUrl}/{testShortCode}");
    }
    
}