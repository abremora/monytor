using FluentValidation;

namespace Monytor.Implementation.Collectors.Sql {
    public class RestApiCollectorValidator : AbstractValidator<RestApiCollector> {
        public RestApiCollectorValidator() {
            RuleFor(x => x.RequestUri)
                .NotNull()
                .WithMessage("The 'Request Uri' must not be empty.");

            RuleFor(x => x.TagName)
                .NotEmpty()
                .WithMessage("The 'Tag Name' name must not be empty.");
        }
    }
}