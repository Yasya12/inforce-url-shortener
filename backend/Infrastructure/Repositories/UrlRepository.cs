using backend.Data;
using backend.Models;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly ApplicationDbContext _context;

        public UrlRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UrlInfo?> GetByCodeAsync(string shortCode)
        {
            return await _context.Urls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        }

        public async Task<UrlInfo?> GetByIdWithUserAsync(int id)
        {
            return await _context.Urls
                .Include(u => u.CreatedBy)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<UrlInfo>> GetAllWithUserAsync()
        {
            return await _context.Urls
                .Include(u => u.CreatedBy)
                .OrderByDescending(u => u.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool> OriginalUrlExistsAsync(string originalUrl)
        {
            return await _context.Urls.AnyAsync(u => u.OriginalUrl == originalUrl);
        }

        public async Task AddAsync(UrlInfo urlInfo)
        {
            await _context.Urls.AddAsync(urlInfo);
        }

        public void Remove(UrlInfo urlInfo)
        {
            _context.Urls.Remove(urlInfo);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
