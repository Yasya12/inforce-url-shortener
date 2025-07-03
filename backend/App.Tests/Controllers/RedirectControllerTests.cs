using Application.Interfaces;
using backend.Controllers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class RedirectControllerTests
{
    [Fact]
    public async Task RedirectToUrl_ReturnsRedirect_WhenUrlExists()
    {
        // Arrange
        var mockRepo = new Mock<IUrlRepository>();
        var url = new UrlInfo { OriginalUrl = "https://example.com" };

        mockRepo.Setup(r => r.GetByCodeAsync("abc123")).ReturnsAsync(url);

        var controller = new RedirectController(mockRepo.Object);

        // Act
        var result = await controller.RedirectToUrl("abc123");

        // Assert
        var redirect = Assert.IsType<RedirectResult>(result);
        Assert.Equal("https://example.com", redirect.Url);
        Assert.True(redirect.Permanent);
    }

    [Fact]
    public async Task RedirectToUrl_ReturnsNotFound_WhenUrlDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IUrlRepository>();
        mockRepo.Setup(r => r.GetByCodeAsync("notfound")).ReturnsAsync((UrlInfo?)null);

        var controller = new RedirectController(mockRepo.Object);

        // Act
        var result = await controller.RedirectToUrl("notfound");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
