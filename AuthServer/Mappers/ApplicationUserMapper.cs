using AuthServer.DAL.Models;
using AuthServer.Models;

namespace AuthServer.Mappers
{
    public static class ApplicationUserMapper
    {
        public static ApplicationUser MapFrom(RegisterUserDto model)
        {
            return new ApplicationUser()
            {
                UserName = model.UserName,
                FamilyName = model.FamilyName,
                GivenName = model.GivenName,
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                PhoneNumberConfirmed = true
            };
        }
    }
}