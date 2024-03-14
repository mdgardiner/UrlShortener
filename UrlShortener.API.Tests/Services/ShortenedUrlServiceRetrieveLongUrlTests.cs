using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using UrlShortener.API.Entities;

namespace UrlShortener.API.Tests.Services;

[TestFixture, Category("UnitTest")]
public class ShortenedUrlServiceRetrieveLongUrlTests : ShortenedUrlServiceTestsBase
{
    [Test]
    public void GivenAShortCode_WhenCallingRetrieveLongUrl_ThenTheShortCodeIsValidated()
    {
        // Arrange
        var testShortCode = "test.shortcode";

        // Act
        Sut.RetrieveLongUrl(testShortCode);

        // Assert
        InputValidatorMock.Verify(x => x.ShortCodeIsValid(testShortCode, ShortCodeLength), Times.Once);
    }

    [Test]
    public void GivenAShortCodeAndValidationFails_WhenCallingRetrieveLongUrl_ThenTheShortCodeIsValidated()
    {
        // Arrange
        var testShortCode = "test.shortcode";
        InputValidatorMock
            .Setup(x => x.ShortCodeIsValid(It.IsAny<string>(), It.IsAny<int>()))
            .Returns(false);

        // Act / Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            Sut.RetrieveLongUrl(testShortCode));

        exception?.Message.Should().Be("test.shortcode must not be empty and be 7 characters long");
    }

    [Test]
    public void GivenAShortCodeForAnExistingUrl_WhenCallingRetrieveLongUrl_ThenTheLongUrlIsReturned()
    {
        // Arrange
        var testLongUrl = "http://test.com/test";
        var testShortCode = "ABCDEFG";
        
        ShortenedUrlRepositoryMock.Setup(x =>
                x.SearchForSingleOrDefault(
                    It.IsAny<Expression<Func<ShortenedUrl, bool>>>()))
            .Returns(new ShortenedUrl{ LongUrl = testLongUrl, ShortCode = testShortCode });

        // Act
        var result = Sut.RetrieveLongUrl(testShortCode);

        // Assert
        ShortenedUrlRepositoryMock.Verify(x => 
            x.SearchForSingleOrDefault(It.IsAny<Expression<Func<ShortenedUrl, bool>>>()), Times.Once);
        result.Should().Be(testLongUrl);
    }

    [Test]
    public void GivenAShortCodeThatDoesntExist_WhenCallingRetrieveLongUrl_ThenNullIsReturned()
    {
        // Arrange
        var testShortCode = "ABCDEFG";
        
        ShortenedUrlRepositoryMock.Setup(x =>
                x.SearchForSingleOrDefault(
                    It.IsAny<Expression<Func<ShortenedUrl, bool>>>()))
            .Returns((ShortenedUrl)default!);
        
        // Act
        var result = Sut.RetrieveLongUrl(testShortCode);
        
        // Assert
        ShortenedUrlRepositoryMock.Verify(x => 
            x.SearchForSingleOrDefault(It.IsAny<Expression<Func<ShortenedUrl, bool>>>()), Times.Once);
        result.Should().BeNull();
    }
}