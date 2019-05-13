using FluentValidation;
using Monytor.Core.Configurations;
using Monytor.Implementation.Collectors.Sql;

namespace Monytor.Implementation.Collectors.RavenDb {

    public class StartingWithCollector : Collector {
        private static readonly StartingWithCollectorValidator Validator = new StartingWithCollectorValidator();

        public string StartingWith { get; set; }
        public DatabaseSource Source { get; set; } = new DatabaseSource();
        public override string GroupName { get; set; } = "StartingDocumentId";

        public override void ValidateAndThrow() {
            base.ValidateAndThrow();
            Validator.ValidateAndThrow(this);
        }
    }
}