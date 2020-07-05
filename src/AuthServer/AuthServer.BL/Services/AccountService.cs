using System.Security.Claims;
using System.Threading.Tasks;
using AuthServer.BL.Exceptions;
using AuthServer.BL.Extensions;
using AuthServer.BL.Interfaces;
using AuthServer.BL.Models;
using AuthServer.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.BL.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserModel> CreateUserAsync(CreateUserModel user)
        {
            var appUser = user.MapToApplicationUser();
            
            var result = await _userManager.CreateAsync(appUser);
            if (result.Succeeded)
            {
                result = await _userManager.AddPasswordAsync(appUser, user.Password);
            }

            if (result.Succeeded)
                return appUser?.MapToUserModel();
            
            throw new UserCreateException(result.Errors);
        }

        public async Task<UserModel> GetUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.MapToUserModel();
        }
        
        public async Task<IdentityResult> UpdateUserAsync(ClaimsPrincipal principal, UpdateUserModel updateUserModel)
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
                throw new UserNotFoundException();
            return await _userManager.UpdateAsync(user.UpdateFrom(updateUserModel));
        }
        
        public async Task<IdentityResult> DeleteUserAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
                throw new UserNotFoundException();
            return await _userManager.DeleteAsync(user);
        }
    }
}