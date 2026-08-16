using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Features.Products.Requests.Commands;

namespace Task_Application.Features.Products.Validation
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommandRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.CreateProduct)
                .NotNull()
                .WithMessage("Product data is required.");

            RuleFor(p => p.CreateProduct.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Name cannot be empty.");

            RuleFor(p => p.CreateProduct.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.");

            RuleFor(p => p.CreateProduct.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity must be Greater Than Or Equal zero.");
        }
    }
}
