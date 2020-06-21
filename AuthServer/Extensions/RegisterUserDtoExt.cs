using AuthServer.BL.Models;
using AuthServer.Models;

namespace AuthServer.Extensions
{
    public static class RegisterUserDtoExt
    {
        public static CreateUserModel MapToUserCreateModel(this RegisterUserDto model)
        {
            return new CreateUserModel()
            {
                UserName = model.UserName,
                Password = model.Password,
                FamilyName = model.FamilyName,
                GivenName = model.GivenName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };
        }
    }
}