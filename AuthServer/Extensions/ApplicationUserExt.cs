using AuthServer.DAL.Models;
using AuthServer.Models;

namespace AuthServer.Extensions
{
    public static class ApplicationUserExt
    {
        public static ApplicationUser UpdateFrom(this ApplicationUser user, UpdateUserDto model)
        {
            user.FamilyName = model.FamilyName;
            user.GivenName = model.GivenName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;

            return user;
        }
    }
}