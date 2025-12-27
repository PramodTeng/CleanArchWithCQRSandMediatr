using FluentValidation;

namespace CleanArchWithCQRSandMediatr.Application.Blogs.Commands.CreateBlog;

// Validator for CreateBlogCommand: declares rules that must pass before the command is handled.
public class CreateBlogCommandValidator : AbstractValidator<CreateBlogCommand>
{
    public CreateBlogCommandValidator()
    {
        // Name: required and limited to 200 chars
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 Characters");

        // Description: required
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is Required");

        // Author: required and limited to 20 chars
        RuleFor(v => v.Author)
            .NotEmpty().WithMessage("Author is required")
            .MaximumLength(20).WithMessage("Name must not exceed 20 characters.");

    }

}