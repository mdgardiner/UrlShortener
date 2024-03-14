using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UrlShortener.API.Entities;
using UrlShortener.API.Repositories.Interfaces;
using UrlShortener.API.Services;
using UrlShortener.API.Services.Interfaces;
using UrlShortener.API.Settings;

namespace UrlShortener.API.Tests.Services;

public abstract class ShortenedUrlServiceTestsBase
{
    protected const string ShortCodePrefixUrl = "http://short.com";
    protected const int ShortCodeLength = 7;
    
    protected Mock<IInputValidator> InputValidatorMock = default!;
    protected Mock<IRepository<ShortenedUrl>> ShortenedUrlRepositoryMock = default!;
    protected Mock<IShortCodeUrlPrefixer> ShortCodeUrlPrefixerMock = default!;
    protected Mock<IShortCodeGenerator> ShortCodeGeneratorMock = default!;
    protected Mock<IOptions<ApplicationSettings>> ApplicationSettingsMock = default!;
    protected Mock<ILogger<ShortenedUrlService>> LoggerMock = default!;
    
    protected ShortenedUrlService Sut = default!;

    [SetUp]
    protected void Setup()
    {
        InputValidatorMock = new Mock<IInputValidator>();
        InputValidatorMock.Setup(x => x.UrlIsValid(It.IsAny<string>())).Returns(true);
        InputValidatorMock.Setup(x => x.ShortCodeIsValid(
            It.IsAny<string>(), 
            It.IsAny<int>()))
            .Returns(true);

        ShortenedUrlRepositoryMock = new Mock<IRepository<ShortenedUrl>>();

        ShortCodeUrlPrefixerMock = new Mock<IShortCodeUrlPrefixer>();
        ShortCodeUrlPrefixerMock.Setup(x => 
                x.GetPrefixedUrl(It.IsAny<string>()))
            .Returns<string>(shortcode => new Uri(string.Join('/', ShortCodePrefixUrl, shortcode)));

        ShortCodeGeneratorMock = new Mock<IShortCodeGenerator>();

        ApplicationSettingsMock = new Mock<IOptions<ApplicationSettings>>();
        ApplicationSettingsMock.Setup(x => x.Value)
            .Returns(new ApplicationSettings
            {
                ShortCodeUrlPrefix = ShortCodePrefixUrl,
                ShortCodeLength = ShortCodeLength
            });
        
        LoggerMock = new Mock<ILogger<ShortenedUrlService>>();

        Sut = new ShortenedUrlService(
            InputValidatorMock.Object, 
            ShortenedUrlRepositoryMock.Object,
            ShortCodeUrlPrefixerMock.Object,
            ShortCodeGeneratorMock.Object,
            ApplicationSettingsMock.Object,
            LoggerMock.Object);
    }
}