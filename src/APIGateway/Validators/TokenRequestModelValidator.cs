using APIGateway.Models;
using FluentValidation;

namespace APIGateway.Validators
{
    public class RequestTokenModelValidator : AbstractValidator<RequestTokenModel>
    {
        public RequestTokenModelValidator()
        {
            RuleFor(i => i.UserName).NotNull().MinimumLength(2);
            RuleFor(i => i.Password).NotNull().MinimumLength(6);
        }
    }
}