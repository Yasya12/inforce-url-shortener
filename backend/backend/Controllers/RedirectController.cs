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
        // Використовуємо параметр urlRepository напряму з основного конструктора
        var url = await urlRepository.GetByCodeAsync(shortCode);

        if (url != null)
        {
            // Постійний редірект (301) краще для SEO
            return RedirectPermanent(url.OriginalUrl);
        }

        return NotFound();
    }
}