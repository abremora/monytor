using FluentValidation;
using Monytor.Implementation.Collectors.RavenDb;

namespace Monytor.Implementation.Collectors.Sql {
    public class AllCollectionCollectorValidator : AbstractValidator<AllCollectionCollector> {
        public AllCollectionCollectorValidator() {
            RuleFor(x => x.Source)
                .NotNull()
                .WithMessage("The 'Source' must not be provided.");

            RuleFor(x => x.Source.Database)
                .NotEmpty()
                .WithMessage("The 'Database' must not be provided.");

            RuleFor(x => x.Source.Url)
                .NotEmpty()
                .WithMessage("The 'Url' name must not be empty.");
        }
    }
}