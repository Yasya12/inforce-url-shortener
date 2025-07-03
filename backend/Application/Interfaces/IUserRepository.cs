using backend.Models;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<ApplicationUser?> FindByIdAsync(string userId);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
    }
}
