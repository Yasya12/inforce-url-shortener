using backend.Services;
using Xunit;

public class UrlShorteningServiceTests
{
    [Theory]
    [InlineData(0, "a")]
    [InlineData(1, "b")]
    [InlineData(62, "ba")]
    public void GenerateShortCode_ReturnsCorrectCode(int id, string expected)
    {
        // Arrange
        var service = new UrlShorteningService();

        // Act
        var result = service.GenerateShortCode(id);

        // Assert
        Assert.Equal(expected.Length, result.Length); // Можна перевіряти більше
    }
}
