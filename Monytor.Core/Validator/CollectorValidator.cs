using FluentValidation;
using Monytor.Core.Configurations;

namespace Monytor.Core.Validator {
    public class CollectorValidator : AbstractValidator<Collector> {
        public CollectorValidator() {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("The 'Id' must not be empty.");

            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .WithMessage("The 'Display Name' name must not be empty.");

            RuleFor(x => x.GroupName)
                .NotEmpty()
                .WithMessage("The 'Group Name' must not be empty.");
        }
    }

    
}
