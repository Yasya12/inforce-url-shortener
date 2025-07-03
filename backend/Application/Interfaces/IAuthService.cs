using backend.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<(string? Token, string? ErrorMessage)> LoginAsync(LoginDto loginDto);
    Task LogoutAsync();
}