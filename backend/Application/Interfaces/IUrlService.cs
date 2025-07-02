using backend.DTOs;
using System.Security.Claims;

namespace Application.Interfaces;

public interface IUrlService
{
    Task<IEnumerable<UrlInfoDto>> GetAllUrlsAsync(string scheme, string host);
    Task<UrlInfoDto?> GetUrlByIdAsync(int id, string scheme, string host);
    Task<(UrlInfoDto? CreatedUrl, string? ErrorMessage)> CreateShortUrlAsync(CreateUrlDto createUrlDto,
        ClaimsPrincipal user, string scheme, string host);
    Task<(bool Success, string? ErrorMessage)> DeleteUrlAsync(int id, ClaimsPrincipal user);
}