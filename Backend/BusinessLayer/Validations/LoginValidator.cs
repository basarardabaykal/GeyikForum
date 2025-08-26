using BusinessLayer.Dtos.Auth;
using FluentValidation;

namespace BusinessLayer.Validations;

public class LoginValidator : AbstractValidator<LoginRequestDto>
{
  public LoginValidator()
  {
    RuleFor(x => x.Email)
      .NotEmpty().WithMessage("Email is required.")
      .EmailAddress().WithMessage("Invalid email.");

    RuleFor(x => x.Password)
      .NotEmpty().WithMessage("Password is required.");
  }
}