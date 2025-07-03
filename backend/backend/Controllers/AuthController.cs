using Application.Interfaces;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var (token, errorMessage) = await authService.LoginAsync(loginDto);

        if (errorMessage is not null)
        {
            return Unauthorized(new { message = errorMessage });
        }

        return Ok(new { token });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await authService.LogoutAsync();
        return Ok(new { message = "Logged out" });
    }
}