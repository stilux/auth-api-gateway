using FluentValidation;
using ProductAPI.Models;

namespace ProductAPI.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(u => u.Name).NotNull().MinimumLength(2);
            RuleFor(u => u.VendorCode).NotNull().MinimumLength(2);
            RuleFor(u => u.Vendor).NotNull().MinimumLength(2);
        }
    }
}