using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Features.Users.Requests.Commands;

namespace Task_Application.Features.Users.Validation
{
    public class LoginUserValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.UserLoginDto)
                .NotNull()
                .WithMessage("Login data is required.");

            RuleFor(x => x.UserLoginDto.UsernameOrEmail)
                .NotEmpty()
                .WithMessage("Username or Email is required.");
                
            RuleFor(x => x.UserLoginDto.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");
        }
    }
}
