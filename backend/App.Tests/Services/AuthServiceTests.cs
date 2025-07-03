using Application.Interfaces;
using Application.Services;
using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using Xunit;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IConfiguration> _configMock = new();
    private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;

    public AuthServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null).Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            null, null, null, null
        );
    }

    [Fact]
    public async Task LoginAsync_ReturnsToken_WhenCredentialsAreValid()
    {
        // Arrange
        var user = new ApplicationUser { Id = "1", Email = "test@example.com" };
        var dto = new LoginDto { Email = "test@example.com", Password = "password" };

        _userRepositoryMock.Setup(r => r.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
        _userRepositoryMock.Setup(r => r.CheckPasswordAsync(user, dto.Password)).ReturnsAsync(true);
        _userRepositoryMock.Setup(r => r.GetRolesAsync(user)).ReturnsAsync(new[] { "User" });

        _configMock.Setup(c => c["Jwt:Key"]).Returns("your_secure_key_here_1234567890!!");
        _configMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
        _configMock.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");

        var authService = new AuthService(_userRepositoryMock.Object, _configMock.Object, _signInManagerMock.Object);

        // Act
        var (token, error) = await authService.LoginAsync(dto);

        // Assert
        Assert.NotNull(token);
        Assert.Null(error);
    }
}
