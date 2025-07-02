using Application.Interfaces;
using backend.DTOs;
using backend.Models;
using System.Security.Claims;

namespace Application.Services;

public class UrlService(
    IUrlRepository urlRepository,
    IUserRepository userRepository,
    IUrlShorteningService shorteningService) : IUrlService
{
    public async Task<IEnumerable<UrlInfoDto>> GetAllUrlsAsync(string scheme, string host)
    {
        var urls = await urlRepository.GetAllWithUserAsync();
        // Логіку мапінгу можна винести в окремий клас-мапер, але для простоти залишимо тут
        return urls.Select(u => MapToDto(u, scheme, host));
    }

    public async Task<UrlInfoDto?> GetUrlByIdAsync(int id, string scheme, string host)
    {
        var url = await urlRepository.GetByIdWithUserAsync(id);
        return url is null ? null : MapToDto(url, scheme, host);
    }

    public async Task<(UrlInfoDto? CreatedUrl, string? ErrorMessage)> CreateShortUrlAsync(CreateUrlDto createUrlDto, ClaimsPrincipal userClaims, string scheme, string host)
    {
        if (await urlRepository.OriginalUrlExistsAsync(createUrlDto.OriginalUrl))
        {
            return (null, "This URL has already been shortened.");
        }

        var userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userRepository.FindByIdAsync(userId);

        var newUrl = new UrlInfo
        {
            OriginalUrl = createUrlDto.OriginalUrl,
            CreatedById = userId,
            ShortCode = "pending"
        };

        await urlRepository.AddAsync(newUrl);
        await urlRepository.SaveChangesAsync();

        newUrl.ShortCode = shorteningService.GenerateShortCode(newUrl.Id);
        await urlRepository.SaveChangesAsync();

        var dto = MapToDto(newUrl, scheme, host, user?.Email);
        return (dto, null);
    }

    public async Task<(bool Success, string? ErrorMessage)> DeleteUrlAsync(int id, ClaimsPrincipal userClaims)
    {
        var urlToDelete = await urlRepository.GetByIdWithUserAsync(id);
        if (urlToDelete is null)
        {
            return (false, "NotFound");
        }

        var currentUserId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!userClaims.IsInRole("Admin") && urlToDelete.CreatedById != currentUserId)
        {
            return (false, "Forbidden");
        }

        urlRepository.Remove(urlToDelete);
        await urlRepository.SaveChangesAsync();

        return (true, null);
    }

    // Приватний метод для мапінгу, щоб уникнути дублювання коду
    private UrlInfoDto MapToDto(UrlInfo url, string scheme, string host, string? email = null)
    {
        return new UrlInfoDto
        {
            Id = url.Id,
            OriginalUrl = url.OriginalUrl,
            ShortUrl = $"{scheme}://{host}/{url.ShortCode}",
            CreatedDate = url.CreatedDate,
            CreatedByEmail = email ?? url.CreatedBy?.Email
        };
    }
}