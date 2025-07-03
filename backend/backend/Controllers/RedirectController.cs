using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("/")]
public class RedirectController(IUrlRepository urlRepository) : ControllerBase
{
    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToUrl(string shortCode)
    {
        var url = await urlRepository.GetByCodeAsync(shortCode);

        if (url != null)
        {
            return RedirectPermanent(url.OriginalUrl);
        }

        return NotFound();
    }
}