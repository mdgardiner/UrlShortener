using FluentAssertions;
using UrlShortener.API.Services;
using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Tests.Services;

[TestFixture, Category("UnitTest")]
public class ShortCodeGeneratorTests
{
    private ShortCodeGenerator _sut = default!;

    [SetUp]
    public void Setup()
    {
        _sut = new ShortCodeGenerator();
    }

    [TestCase(1)]
    [TestCase(5)]
    [TestCase(7)]
    [TestCase(10)]
    public void GivenALength_WhenCallingGenerateShortCode_ThenAStringOfCorrectLengthIsReturned(int expectedLength)
    {
        // Act
        var result = _sut.GenerateShortCode(expectedLength);
        
        // Assert
        result.Should().HaveLength(expectedLength);
    }

    [Test]
    public void GivenCalledMultipleTimes_WhenCallingGenerateShortCode_ShouldReturnADifferentStringForEachCall()
    {
        // Arrange
        var length = 7;
        var numberOfCalls = 3;
        
        // Act
        var results = new HashSet<string>();
        for (var index = 0; index < numberOfCalls; index++)
        {
            results.Add(_sut.GenerateShortCode(length));
        }

        // Assert
        results.Should().HaveCount(numberOfCalls);
    }
}