using AuthServer.Models;
using FluentValidation;

namespace AuthServer.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(u => u.FamilyName).NotNull().MinimumLength(2);
            RuleFor(u => u.GivenName).NotNull().MinimumLength(2);
            RuleFor(u => u.Email).NotNull().EmailAddress();
            RuleFor(u => u.PhoneNumber).NotNull().Matches(@"^[0-9\-]{10,12}$");
        }
    }
}