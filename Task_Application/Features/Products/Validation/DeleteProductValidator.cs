using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Features.Products.Requests.Commands;

namespace Task_Application.Features.Products.Validation
{
    public class DeleteProductValidator : AbstractValidator<DeleteProductCommandRequest>
    {
        public DeleteProductValidator()
        {
            RuleFor(p=>p.productId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("Product Id is required.");
        }
    }
}
