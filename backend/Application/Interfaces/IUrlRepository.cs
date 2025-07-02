using backend.Models;

namespace Application.Interfaces
{
    public interface IUrlRepository
    {
        Task<UrlInfo?> GetByCodeAsync(string shortCode);
        Task<UrlInfo?> GetByIdWithUserAsync(int id);
        Task<IEnumerable<UrlInfo>> GetAllWithUserAsync();
        Task<bool> OriginalUrlExistsAsync(string originalUrl);
        Task AddAsync(UrlInfo urlInfo);
        void Remove(UrlInfo urlInfo);
        Task<int> SaveChangesAsync();
    }
}
