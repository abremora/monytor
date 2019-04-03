using FluentValidation;
using Monytor.Implementation.Collectors.NetFramework;

namespace Monytor.Implementation.Collectors.Sql {
    public class PerformanceCounterCollectorValidator : AbstractValidator<PerformanceCounterCollector> {
        public PerformanceCounterCollectorValidator() {
            RuleFor(x => x.Category)
                .NotNull()
                .WithMessage("The 'Category' must not be empty.");

            RuleFor(x => x.Instance)
                .NotEmpty()
                .WithMessage("The 'Instance' name must not be empty.");

            RuleFor(x => x.MachineName)
                .NotEmpty()
                .WithMessage("The 'MachineName' name must not be empty.");

            RuleFor(x => x.Counter)
                .NotEmpty()
                .WithMessage("The 'Counter' name must not be empty.");
        }
    }
}