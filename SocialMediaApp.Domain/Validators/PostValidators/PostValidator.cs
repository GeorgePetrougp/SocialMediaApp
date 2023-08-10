using FluentValidation;
using SocialMediaApp.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApp.Domain.Validators.PostValidators
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(post => post.TextContent)
                .NotNull().WithMessage("Post text content can not be null")
                .NotEmpty().WithMessage("Post text content can not be empty");
        }
    }
}
