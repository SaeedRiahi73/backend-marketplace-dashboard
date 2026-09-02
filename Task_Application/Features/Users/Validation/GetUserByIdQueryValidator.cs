using FluentValidation;
using Task_Application.Features.Users.Requests.Queries;

namespace Task_Application.Features.Users.Validation;

public sealed class GetUserByIdQueryValidator
    : AbstractValidator<GetUserByIdQueryRequest>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(request => request.UserId)
            .NotEmpty()
            .WithMessage("User id is required.");
    }
}
