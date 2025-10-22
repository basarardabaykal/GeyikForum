using BusinessLayer.Dtos.Auth;
using FluentValidation;

namespace BusinessLayer.Validations;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequestDto>
{
  public ResetPasswordValidator()
  {
    RuleFor(x => x.NewPassword)
      .NotEmpty().WithMessage("Password is required.")
      .MinimumLength(6).WithMessage("Password must have at least 6 characters.")
      .MaximumLength(64).WithMessage("Password can not have more than 64 characters.")
      .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
      .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
      .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
      .Matches(@"[^A-Za-z0-9]").WithMessage("Password must contain at least one special character.");

    RuleFor(x => x.ConfirmPassword)
      .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
  }
}