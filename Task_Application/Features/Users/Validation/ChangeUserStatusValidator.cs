using FluentValidation;
using Task_Application.Features.Users.Requests.Commands;

namespace Task_Application.Features.Users.Validation;

public sealed class ChangeUserStatusValidator
    : AbstractValidator<ChangeUserStatusCommandRequest>
{
    public ChangeUserStatusValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty()
            .WithMessage("User id is required.");

        RuleFor(request => request.UserStatus)
            .NotNull()
            .WithMessage("User status data is required.");
    }
}
