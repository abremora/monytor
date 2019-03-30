using FluentValidation;
using Monytor.Core.Models;

namespace Monytor.Core.Validator {
    public class CollectorConfigStoredValidator : AbstractValidator<CollectorConfigStored> {
        public CollectorConfigStoredValidator() {
            RuleFor(x => x.DisplayName)
                .NotEmpty()
                .WithMessage("The 'Display Name' must not be empty.");

            RuleFor(x => x.SchedulerAgentId)
                .NotEmpty()
                .WithMessage("The 'Scheduler Agent Id' must not be empty.");
        }
    }
}
