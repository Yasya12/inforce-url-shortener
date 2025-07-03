using Application.Interfaces;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IConfiguration configuration,
    SignInManager<ApplicationUser> signInManager) : IAuthService
{
    public async Task<(string? Token, string? ErrorMessage)> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepository.FindByEmailAsync(loginDto.Email);

        if (user is null || !await userRepository.CheckPasswordAsync(user, loginDto.Password))
        {
            return (null, "Invalid Credentials");
        }

        // Setting cookies for Razor Pages
        await signInManager.SignInAsync(user, isPersistent: false);


        var tokenString = await GenerateJwtToken(user);
        return (tokenString, null);
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var userRoles = await userRepository.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new Claim("email", user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        claims.Add(new Claim("roles", JsonSerializer.Serialize(userRoles)));

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