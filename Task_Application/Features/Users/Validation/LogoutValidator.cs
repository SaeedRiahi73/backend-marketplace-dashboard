using FluentValidation;
using Task_Application.Features.Users.Requests.Commands;

namespace Task_Application.Features.Users.Validation;

public sealed class LogoutValidator : AbstractValidator<LogoutCommandRequest>
{
    public LogoutValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}
