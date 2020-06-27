using AuthServer.Models;
using FluentValidation;

namespace AuthServer.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(u => u.UserName).NotNull().MinimumLength(2);
            RuleFor(u => u.Password).NotNull().MinimumLength(6);
            RuleFor(u => u.FamilyName).NotNull().MinimumLength(2);
            RuleFor(u => u.GivenName).NotNull().MinimumLength(2);
            RuleFor(u => u.Email).NotNull().EmailAddress();
            RuleFor(u => u.PhoneNumber).NotNull().Matches(@"^[0-9\-]{10,12}$");
        }
    }
}