using FluentValidation;

namespace Monytor.Implementation.Collectors.Sql {
    public class CountCollectorValidator : AbstractValidator<CountBaseCollector> {
        public CountCollectorValidator() {
            RuleFor(x => x.TableName)
                .NotEmpty()
                .WithMessage("The 'Table Name' must not be empty.");

            RuleFor(x => x.ConnectionString)
                .NotEmpty()
                .WithMessage("The 'Connection String' name must not be empty.");
        }
    }
}