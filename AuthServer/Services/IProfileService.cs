using System.Security.Claims;
using System.Threading.Tasks;
using AuthServer.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Services
{
    public interface IProfileService : IdentityServer4.Services.IProfileService
    {
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal principal);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<IdentityResult> AddUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
    }
}