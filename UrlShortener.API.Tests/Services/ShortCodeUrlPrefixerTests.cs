using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using UrlShortener.API.Services;
using UrlShortener.API.Settings;

namespace UrlShortener.API.Tests.Services;

[TestFixture, Category("UnitTest")]
public class ShortCodeUrlPrefixerTests
{
    private const string ShortCodeUrlPrefix = "http://test.com";
    
    private Mock<IOptions<ApplicationSettings>> _applicationSettingsMock = default!;
    
    private ShortCodeUrlPrefixer _sut = default!;

    [SetUp]
    public void Setup()
    {
        _applicationSettingsMock = new Mock<IOptions<ApplicationSettings>>();
        _applicationSettingsMock.Setup(x => x.Value)
            .Returns(new ApplicationSettings
            {
                ShortCodeUrlPrefix = ShortCodeUrlPrefix,
            });

        _sut = new ShortCodeUrlPrefixer(_applicationSettingsMock.Object);
    }

    [Test]
    public void GivenAShortCode_WhenCallingGetPrefixedUrl_ShouldReturnExpectedResult()
    {
        // Arrange
        var testShortCode = "test.short.code";

        // Act
        var result = _sut.GetPrefixedUrl(testShortCode);

        // Assert
        result.Should().Be($"{ShortCodeUrlPrefix}/{testShortCode}");
    }
}