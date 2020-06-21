using AuthServer.BL.Models;
using AuthServer.Models;

namespace AuthServer.Extensions
{
    public static class UpdateUserDtoExt
    {
        public static UpdateUserModel MapToUserUpdateModel(this UpdateUserDto model)
        {
            return new UpdateUserModel()
            {
                FamilyName = model.FamilyName,
                GivenName = model.GivenName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };
        }
    }
}