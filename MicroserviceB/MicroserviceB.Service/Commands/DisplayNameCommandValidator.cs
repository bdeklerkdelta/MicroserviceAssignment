using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace MicroserviceB.Service.Commands
{
    public class DisplayNameCommandValidator : AbstractValidator<DisplayNameCommand>
    {
        public DisplayNameCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("Name cannot be empty.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty.");
            RuleFor(x => x.Name).Cascade(CascadeMode.Stop).NotNull().Custom((name, context) => {
                if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                {
                    context.AddFailure("Name contains invalid characters.");
                }
            });
            RuleFor(x => x.Name).Cascade(CascadeMode.Stop).NotNull().Custom((name, context) => {
                foreach (char c in name)
                {
                    if (!Char.IsLetter(c))
                    {
                        context.AddFailure("Name may only contain letters.");
                        break;
                    }
                }
            });
        }
    }
}
