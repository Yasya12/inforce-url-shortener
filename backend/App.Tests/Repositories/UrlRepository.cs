using backend.Data;
using backend.Models;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

public class UrlRepositoryTests
{
    private async Task<ApplicationDbContext> GetDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
            .Options;

        var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();
        return context;
    }

    [Fact]
    public async Task AddAsync_AddsUrlToDatabase()
    {
        var context = await GetDbContextAsync();
        var repo = new UrlRepository(context);

        var url = new UrlInfo { OriginalUrl = "https://example.com", ShortCode = "abc123", CreatedById = "test-user-id" };
        await repo.AddAsync(url);
        await repo.SaveChangesAsync();

        Assert.Equal(1, await context.Urls.CountAsync());
        Assert.Equal("https://example.com", (await context.Urls.FirstAsync()).OriginalUrl);
    }

    [Fact]
    public async Task GetByCodeAsync_ReturnsUrl_WhenExists()
    {
        var context = await GetDbContextAsync();
        var repo = new UrlRepository(context);

        var url = new UrlInfo { OriginalUrl = "https://example.com", ShortCode = "abc123", CreatedById = "test-user-id" };
        await repo.AddAsync(url);
        await repo.SaveChangesAsync();

        var result = await repo.GetByCodeAsync("abc123");

        Assert.NotNull(result);
        Assert.Equal("https://example.com", result.OriginalUrl);
    }

    [Fact]
    public async Task GetByCodeAsync_ReturnsNull_WhenNotExists()
    {
        var context = await GetDbContextAsync();
        var repo = new UrlRepository(context);

        var result = await repo.GetByCodeAsync("notexist");

        Assert.Null(result);
    }

    [Fact]
    public async Task OriginalUrlExistsAsync_ReturnsTrue_WhenUrlExists()
    {
        var context = await GetDbContextAsync();
        var repo = new UrlRepository(context);

        var url = new UrlInfo { OriginalUrl = "https://example.com", ShortCode = "abc123", CreatedById = "test-user-id" };
        await repo.AddAsync(url);
        await repo.SaveChangesAsync();

        var exists = await repo.OriginalUrlExistsAsync("https://example.com");

        Assert.True(exists);
    }

    [Fact]
    public async Task Remove_RemovesUrlFromDatabase()
    {
        var context = await GetDbContextAsync();
        var repo = new UrlRepository(context);

        var url = new UrlInfo { OriginalUrl = "https://example.com", ShortCode = "abc123", CreatedById = "test-user-id" };
        await repo.AddAsync(url);
        await repo.SaveChangesAsync();

        repo.Remove(url);
        await repo.SaveChangesAsync();

        var count = await context.Urls.CountAsync();
        Assert.Equal(0, count);
    }
}
