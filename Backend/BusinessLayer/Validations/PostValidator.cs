using BusinessLayer.Dtos;
using FluentValidation;

namespace BusinessLayer.Validations;

public class PostValidator : AbstractValidator<PostDto>
{
 public PostValidator()
 {

  // Content is required
  RuleFor(p => p.Content)
   .NotEmpty().WithMessage("Content is required.")
   .MaximumLength(280).WithMessage("Content must not exceed 280 characters.");

  // Title can only be provided for main posts (when ParentId is null)
  RuleFor(p => p.Title)
   .MaximumLength(50).WithMessage("Title cannot exceed 50 characters.")
   .When(p => !string.IsNullOrEmpty(p.Title));

  // Title is required for main posts.
  RuleFor(p => p.Title)
   .NotEmpty()
   .When(p => p.ParentId.HasValue && p.ParentId != Guid.Empty)
   .WithMessage("Title is required.");
 }
}