using Application.Interfaces;
using backend.DTOs;
using backend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;

public class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
{
    public async Task<(string? Token, string? ErrorMessage)> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepository.FindByEmailAsync(loginDto.Email);

        if (user is null || !await userRepository.CheckPasswordAsync(user, loginDto.Password))
        {
            return (null, "Invalid Credentials");
        }

        var tokenString = await GenerateJwtToken(user);
        return (tokenString, null);
    }

    // Цей метод тепер є приватною частиною сервісу
    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var userRoles = await userRepository.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(1);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}