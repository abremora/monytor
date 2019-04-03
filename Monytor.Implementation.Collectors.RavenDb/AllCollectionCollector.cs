using FluentValidation;
using Monytor.Core.Configurations;
using Monytor.Implementation.Collectors.Sql;

namespace Monytor.Implementation.Collectors.RavenDb {
    public class AllCollectionCollector : Collector {
        private  static readonly AllCollectionCollectorValidator Validator = new AllCollectionCollectorValidator();
        public DatabaseSource Source { get; set; } = new DatabaseSource();
        public override string GroupName { get; set; } = "Collection";

        public override void ValidateAndThrow() {
            base.ValidateAndThrow();
            Validator.ValidateAndThrow(this);
        }
    }
}
