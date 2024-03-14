using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using UrlShortener.API.Entities;

namespace UrlShortener.API.Tests.Services;

[TestFixture, Category("UnitTest")]
public class ShortenedUrlServiceCreateShortenedUrlTests : ShortenedUrlServiceTestsBase
{
    [Test]
    public void GivenAUrl_WhenCallingCreateShortenedUrl_ThenUrlIsValidated()
    {
        // Arrange
        var testUrl = "test.url";
        
        // Act
        Sut.GetShortenedUrl(testUrl);

        // Assert
        InputValidatorMock.Verify(x => x.UrlIsValid(testUrl), Times.Once);
    }

    [Test]
    public void GivenAUrlAndValidationFails_WhenCallingCreateShortenedUrl_ThenArgumentExceptionIsThrown()
    {
        // Arrange
        var testUrl = "test.url";
        InputValidatorMock.Setup(x => x.UrlIsValid(It.IsAny<string>())).Returns(false);

        // Act / Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            Sut.GetShortenedUrl(testUrl));

        exception?.Message.Should().Be("test.url is not a valid url");
    }

    [Test]
    public void GivenAUrlThatHasAlreadyBeenShortened_WhenCallingCreateShortenedUrl_ThenReturnsExistingShortUrl()
    {
        // Arrange
        var testLongUrl = "test.long.url";
        var testShortCode = "test.short.code";

        ShortenedUrlRepositoryMock.Setup(x =>
                x.SearchForSingleOrDefault(
                    It.IsAny<Expression<Func<ShortenedUrl, bool>>>()))
            .Returns(new ShortenedUrl{ LongUrl = testLongUrl, ShortCode = testShortCode });

        // Act
        var result = Sut.GetShortenedUrl(testLongUrl);

        // Assert
        ShortenedUrlRepositoryMock.Verify(x => 
            x.SearchForSingleOrDefault(It.IsAny<Expression<Func<ShortenedUrl, bool>>>()), Times.Once);
        ShortCodeUrlPrefixerMock.Verify(x => x.GetPrefixedUrl(testShortCode), Times.Once);
        result.Should().Be($"{ShortCodePrefixUrl}/{testShortCode}");
    }

    [Test]
    public void
        GivenAUrlThatHasNotBeenShortened_WhenCallingCreateShortenedUrl_ThenShortCodeIsCreatedSavedAndUriReturned()
    {
        // Arrange
        var testLongUrl = "test.long.url";
        var testShortCode = "test.short.code";

        ShortCodeGeneratorMock.Setup(x => x.GenerateShortCode(It.IsAny<int>()))
            .Returns(testShortCode);
        
        // Act
        var result = Sut.GetShortenedUrl(testLongUrl);

        // Assert
        ShortCodeGeneratorMock.Verify(x => x.GenerateShortCode(ShortCodeLength), Times.Once);
        ShortCodeUrlPrefixerMock.Verify(x => x.GetPrefixedUrl(testShortCode), Times.Once);
        ShortenedUrlRepositoryMock.Verify(x => x.Insert(
            It.Is<ShortenedUrl>(y => 
                y.LongUrl == testLongUrl && 
                y.ShortCode == testShortCode)),
            Times.Once);
        ShortenedUrlRepositoryMock.Verify(x => x.Commit(), Times.Once);
        result.Should().Be($"{ShortCodePrefixUrl}/{testShortCode}");
    }

    [Test]
    public void GivenAShortCodeIsGeneratedThatIsNotUnique_WhenCallingCreateShortenedURL_ThenShortCodeIsRegenerated()
    {
        // Arrange
        var testLongUrl = "test.long.url";
        var testExistingShortCode = "existing.short.code";
        var testNewShortCode = "new.short.code";

        ShortCodeGeneratorMock.SetupSequence(x => x.GenerateShortCode(It.IsAny<int>()))
            .Returns(testExistingShortCode)
            .Returns(testNewShortCode);
        
        ShortenedUrlRepositoryMock.SetupSequence(x =>
                x.SearchForSingleOrDefault(
                    It.IsAny<Expression<Func<ShortenedUrl, bool>>>()))
            .Returns((ShortenedUrl)default!)
            .Returns(new ShortenedUrl{ LongUrl = testLongUrl, ShortCode = testExistingShortCode })
            .Returns((ShortenedUrl)default!);
        
        // Act
        var result = Sut.GetShortenedUrl(testLongUrl);

        // Assert
        ShortCodeGeneratorMock.Verify(x => x.GenerateShortCode(ShortCodeLength), Times.Exactly(2));
        ShortCodeUrlPrefixerMock.Verify(x => x.GetPrefixedUrl(testNewShortCode), Times.Once);
        ShortenedUrlRepositoryMock.Verify(x => x.Insert(
                It.Is<ShortenedUrl>(y => 
                    y.LongUrl == testLongUrl && 
                    y.ShortCode == testNewShortCode)),
            Times.Once);
        ShortenedUrlRepositoryMock.Verify(x => x.Commit(), Times.Once);
        result.Should().Be($"{ShortCodePrefixUrl}/{testNewShortCode}");
    }
}