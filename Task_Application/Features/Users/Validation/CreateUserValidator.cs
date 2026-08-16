using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Features.Users.Requests.Commands;

namespace Task_Application.Features.Users.Validation
{
    public class CreateUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.CreateUser)
                .NotNull()
                .WithMessage("Login data is required.");

            RuleFor(x => x.CreateUser.Username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MinimumLength(3)
                .WithMessage("Username must be at least 3 characters")
                .MaximumLength(50)
                .WithMessage("Username cannot exceed 50 characters");

            RuleFor(x => x.CreateUser.Email)
                .NotEmpty()
                .WithMessage("Email is required.");

            RuleFor(x => x.CreateUser.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");
        }
    }
}
