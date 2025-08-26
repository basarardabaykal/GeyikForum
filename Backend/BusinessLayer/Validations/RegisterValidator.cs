using BusinessLayer.Dtos.Auth;
using FluentValidation;

namespace BusinessLayer.Validations;

public class RegisterValidator : AbstractValidator<RegisterRequestDto>
{
  public RegisterValidator()
  {
    RuleFor(x => x.Email)
      .NotEmpty().WithMessage("Email is required.")
      .EmailAddress().WithMessage("Invalid email.:D");

    RuleFor(x => x.Password)
      .NotEmpty().WithMessage("Password is required.")
      .MinimumLength(6).WithMessage("Password must have at least 6 characters.")
      .MaximumLength(64).WithMessage("Password can not have more than 64 characters.")
      .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.:D")
      .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
      .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
      .Matches(@"[^A-Za-z0-9]").WithMessage("Password must contain at least one special character.");

    RuleFor(x => x.ConfirmPassword)
      .Equal(x => x.Password).WithMessage("Passwords do not match:D");

    RuleFor(x => x.Nickname)
      .NotEmpty().WithMessage("Nickname is required.")
      .MinimumLength(3).WithMessage("Nickname must have at least 3 characters.:D")
      .MaximumLength(20).WithMessage("Nickname can not have more than 20 characters.")
      .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Nickname can only contain letters, numbers, underscores, or hyphens.")
      .Matches(@"^(?![_-])(?!.*[_-]{2})(?!.*[_-]$).+$").WithMessage("Nickname cannot start/end with or contain consecutive special characters."); 
  }
}