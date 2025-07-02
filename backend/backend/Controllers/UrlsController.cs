using Application.Interfaces;
using backend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlsController(IUrlService urlService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UrlInfoDto>>> GetUrls()
    {
        var urls = await urlService.GetAllUrlsAsync(Request.Scheme, Request.Host.ToString());
        return Ok(urls);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UrlInfoDto>> GetUrl(int id)
    {
        var urlDto = await urlService.GetUrlByIdAsync(id, Request.Scheme, Request.Host.ToString());
        return urlDto is null ? NotFound() : Ok(urlDto);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<UrlInfoDto>> CreateShortUrl([FromBody] CreateUrlDto createUrlDto)
    {
        var (createdUrl, errorMessage) = await urlService.CreateShortUrlAsync(createUrlDto, User, Request.Scheme, Request.Host.ToString());

        if (errorMessage is not null)
        {
            return Conflict(new { message = errorMessage });
        }

        return CreatedAtAction(nameof(GetUrl), new { id = createdUrl!.Id }, createdUrl);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteUrl(int id)
    {
        var (success, errorMessage) = await urlService.DeleteUrlAsync(id, User);

        if (!success)
        {
            return errorMessage switch
            {
                "NotFound" => NotFound(),
                "Forbidden" => Forbid(),
                _ => BadRequest()
            };
        }

        return NoContent();
    }
}