using FluentValidation;
using Monytor.Implementation.Collectors.RavenDb;

namespace Monytor.Implementation.Collectors.Sql {
    public class CollectionCollectorValidator : AbstractValidator<CollectionCollector> {
        public CollectionCollectorValidator() {
            RuleFor(x => x.Source)
                .NotNull()
                .WithMessage("The 'Source' must not be provided.");

            RuleFor(x => x.Source.Database)
                .NotEmpty()
                .WithMessage("The 'Database' must not be provided.");

            RuleFor(x => x.Source.Url)
                .NotEmpty()
                .WithMessage("The 'Url' name must not be empty.");

            RuleFor(x => x.CollectionName)
                .NotEmpty()
                .WithMessage("The 'Collection Name' name must not be empty.");
        }
    }
}