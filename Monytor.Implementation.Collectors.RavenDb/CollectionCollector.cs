using FluentValidation;
using Monytor.Core.Configurations;
using Monytor.Implementation.Collectors.Sql;

namespace Monytor.Implementation.Collectors.RavenDb {

    public class CollectionCollector : Collector {
        private static readonly CollectionCollectorValidator Validator = new CollectionCollectorValidator();

        public string CollectionName { get; set; }
        public DatabaseSource Source { get; set; } = new DatabaseSource();
        public override string GroupName { get; set; } = "Collection";

        public override void ValidateAndThrow() {
            base.ValidateAndThrow();
            Validator.ValidateAndThrow(this);
        }
    }
}