using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Features.Users.Requests.Commands;
using Task_Domain.Enums;

namespace Task_Application.Features.Users.Validation
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommandRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.CreateUser)
                .NotNull()
                .WithMessage("User data is required.");

            RuleFor(x => x.CreateUser.Username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .MinimumLength(3)
                .WithMessage("Username must be at least 3 characters.")
                .MaximumLength(50)
                .WithMessage("Username cannot exceed 50 characters.")
                .Matches("^[a-zA-Z0-9._-]+$")
                .WithMessage(
                    "Username can only contain letters, numbers, dots, underscores, and hyphens.");

            RuleFor(x => x.CreateUser.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email format is invalid.")
                .MaximumLength(50)
                .WithMessage("Email cannot exceed 50 characters.");

            RuleFor(x => x.CreateUser.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(100)
                .WithMessage("Password cannot exceed 100 characters.");

            RuleFor(x => x.CreateUser.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Password confirmation is required.")
                .Equal(x => x.CreateUser.Password)
                .WithMessage("Password and confirmation password must match.");

            RuleFor(x => x.CreateUser.Role)
                .Must(role => role is null || role == UserRole.ProductManager)
                .WithMessage("The selected role is not allowed for user creation.");
        }
    }
}
