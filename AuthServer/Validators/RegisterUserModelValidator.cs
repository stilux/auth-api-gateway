using AuthServer.Models;
using FluentValidation;

namespace AuthServer.Validators
{
    public class RegisterUserModelValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserModelValidator()
        {
            RuleFor(u => u.UserName).MinimumLength(2);
            RuleFor(u => u.Password).MinimumLength(6);
            RuleFor(u => u.FamilyName).MinimumLength(2);
            RuleFor(u => u.GivenName).MinimumLength(2);
            RuleFor(u => u.Email).NotNull().EmailAddress();
            RuleFor(u => u.PhoneNumber).Matches(@"^[0-9\-]{10,12}$");
        }
    }
}