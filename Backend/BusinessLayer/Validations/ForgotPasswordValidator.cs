using BusinessLayer.Dtos.Auth;
using FluentValidation;

namespace BusinessLayer.Validations;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequestDto>
{
  public ForgotPasswordValidator()
  {
    RuleFor(x => x.Email)
      .NotEmpty().WithMessage("Email is required.")
      .EmailAddress().WithMessage("Invalid email.");
  }
}