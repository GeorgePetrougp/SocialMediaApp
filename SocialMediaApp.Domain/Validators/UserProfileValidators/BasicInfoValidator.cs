using FluentValidation;
using SocialMediaApp.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SocialMediaApp.Domain.Validators.UserProfileValidators
{
    public class BasicInfoValidator : AbstractValidator<BasicInfo>
    {
        public BasicInfoValidator()
        {
            RuleFor(info => info.FirstName)
                .NotNull().WithMessage("First name is required. It is currently null")
                .MinimumLength(3).WithMessage("First name must be at least 3 characters long")
                .MaximumLength(50).WithMessage("First name must contain at most 50 characters");

            RuleFor(info => info.LastName)
                .NotNull().WithMessage("Last name is required. It is currently null")
                .MinimumLength(3).WithMessage("Last name must be at least 3 characters long")
                .MaximumLength(50).WithMessage("Last name must contain at most 50 characters");

            RuleFor(info => info.EmailAdress)
                .NotNull().WithMessage("Email Address is required")
                .EmailAddress().WithMessage("Not a correct email format");

            RuleFor(info => info.DateOfBirth)
                .InclusiveBetween(new DateTime(DateTime.Now.AddYears(-125).Ticks), new DateTime(DateTime.Now.AddYears(-18).Ticks))
                .WithMessage("You must be over 18 years old and under 130");

        }
    }
}
