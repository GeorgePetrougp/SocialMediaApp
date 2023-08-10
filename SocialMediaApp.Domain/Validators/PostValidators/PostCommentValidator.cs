using FluentValidation;
using SocialMediaApp.Domain.Aggregates.PostAggregate;


namespace SocialMediaApp.Domain.Validators.PostValidators
{
    public class PostCommentValidator : AbstractValidator<PostComment>
    {
        public PostCommentValidator()
        {
            RuleFor(pc => pc.Text)
                .NotNull().WithMessage("Comment text should not be null")
                .NotEmpty().WithMessage("Comment should not be empty")
                .MaximumLength(1000)
                .MinimumLength(1);
        }
    }
}
