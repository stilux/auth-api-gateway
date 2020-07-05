using AuthServer.BL.Models;
using AuthServer.DAL.Models;

namespace AuthServer.BL.Extensions
{
    public static class CreateUserModelExt
    {
        public static ApplicationUser MapToApplicationUser(this CreateUserModel model)
        {
            return new ApplicationUser()
            {
                UserName = model.UserName,
                FamilyName = model.FamilyName,
                GivenName = model.GivenName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
        }
    }
}