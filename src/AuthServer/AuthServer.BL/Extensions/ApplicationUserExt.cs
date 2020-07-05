using AuthServer.BL.Models;
using AuthServer.DAL.Models;

namespace AuthServer.BL.Extensions
{
    public static class ApplicationUserExt
    {
        public static ApplicationUser UpdateFrom(this ApplicationUser user, UpdateUserModel model)
        {
            user.FamilyName = model.FamilyName;
            user.GivenName = model.GivenName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;

            return user;
        }
        
        public static UserModel MapToUserModel(this ApplicationUser user)
        {
            return new UserModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FamilyName = user.FamilyName,
                GivenName = user.GivenName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
        }
    }
}