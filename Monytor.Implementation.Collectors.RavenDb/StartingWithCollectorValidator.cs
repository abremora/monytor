using FluentValidation;
using Monytor.Implementation.Collectors.RavenDb;

namespace Monytor.Implementation.Collectors.Sql {
    public class StartingWithCollectorValidator : AbstractValidator<StartingWithCollector> {
        public StartingWithCollectorValidator() {
            RuleFor(x => x.Source)
                .NotNull()
                .WithMessage("The 'Source' must not be provided.");

            RuleFor(x => x.Source.Database)
                .NotEmpty()
                .WithMessage("The 'Database' must not be provided.");

            RuleFor(x => x.Source.Url)
                .NotEmpty()
                .WithMessage("The 'Url' name must not be empty.");

            RuleFor(x => x.StartingWith)
                .NotEmpty()
                .WithMessage("The 'Starting With' name must not be empty.");
        }
    }
}