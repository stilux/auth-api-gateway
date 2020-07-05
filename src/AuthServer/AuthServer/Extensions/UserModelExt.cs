using AuthServer.BL.Models;
using AuthServer.Models;

namespace AuthServer.Extensions
{
    public static class UserModelExt
    {
        public static UserDto MapToUserDto(this UserModel model)
        {
            return new UserDto()
            {
                Id = model.Id,
                UserName = model.UserName,
                FamilyName = model.FamilyName,
                GivenName = model.GivenName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };
        }
    }
}