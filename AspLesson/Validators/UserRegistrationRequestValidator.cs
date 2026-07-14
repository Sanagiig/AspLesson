using AspLesson.Data.Requests;
using FluentValidation;

namespace AspLesson.Validators;

public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
{
    public UserRegistrationRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(4).WithMessage("{PropertyName} must be at least {MinLength} characters.")
            .Must(p => p.IsNormalized()).WithMessage("{PropertyName} should be all letters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .EmailAddress().WithMessage("Invalid {PropertyName} address.");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MinimumLength(6).WithMessage("{PropertyName} must be at least {MinLength} characters.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Please confirm your password.")
            .Equal(x => x.Password).WithMessage("Passwords do not match.");
    }
}
