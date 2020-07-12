using FluentValidation;

namespace NetCoreWebTemplate.Application.Identity.Commands.CreateAccount
{
    public class CreateAccountCommandValidator: AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email field is required!")
                .EmailAddress().WithMessage("Invalid email address!");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password field is required!");

            RuleFor(v => v.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password field is required!")
                .NotEqual(v => v.Password).WithMessage("Confirm Password must match Password!");
        }
    }
}