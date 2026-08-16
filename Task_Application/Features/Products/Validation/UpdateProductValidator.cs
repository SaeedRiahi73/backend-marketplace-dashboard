using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Common.Responses;
using Task_Application.Features.Products.Requests.Commands;

namespace Task_Application.Features.Products.Validation
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommandRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(p => p.updateProductDto)
              .NotNull()
              .WithMessage("Product data is required.");

            RuleFor(p => p.updateProductDto.Id)
                .NotNull()
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("Product Id is required.");

            RuleFor(p => p.updateProductDto.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("Name cannot be empty.");

            RuleFor(p => p.updateProductDto.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.");

            RuleFor(p => p.updateProductDto.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity must be Greater Than Or Equal zero.");

        }
    }
}
