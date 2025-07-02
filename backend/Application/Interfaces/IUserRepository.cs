using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
