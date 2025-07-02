namespace Application.Interfaces
{
    public interface IUrlShorteningService
    {
        string GenerateShortCode(int id);
    }
}
