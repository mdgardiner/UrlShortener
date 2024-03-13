using Microsoft.AspNetCore.Mvc;
using UrlShortener.API.Models.Dtos;
using UrlShortener.API.Services.Interfaces;

namespace UrlShortener.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    private readonly ILogger<ApiController> _logger;
    private readonly IShortenedUrlService _shortenedUrlService;

    public ApiController(
        ILogger<ApiController> logger,
        IShortenedUrlService shortenedUrlService)
    {
        _logger = logger;
        _shortenedUrlService = shortenedUrlService;
    }

    [HttpPost]
    [Route("shorten")]
    public IActionResult GetShortenedUrl(CreateShortenedUrlDto shortenedUrlDto)
    {
        _logger.LogInformation("Request received to shorten '{url}'", shortenedUrlDto.LongUrl);

        try
        {
            var result = _shortenedUrlService.GetShortenedUrl(shortenedUrlDto.LongUrl);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest();
        }
        
    }
}