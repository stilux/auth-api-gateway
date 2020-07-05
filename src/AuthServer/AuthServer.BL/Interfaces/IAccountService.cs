using System.Security.Claims;
using System.Threading.Tasks;
using AuthServer.BL.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.BL.Interfaces
{
    public interface IAccountService
    {
        Task<UserModel> CreateUserAsync(CreateUserModel user);
        Task<UserModel> GetUserAsync(string userId);
        Task<IdentityResult> UpdateUserAsync(ClaimsPrincipal principal, UpdateUserModel updateUserModel);
        Task<IdentityResult> DeleteUserAsync(ClaimsPrincipal principal);
    }
}