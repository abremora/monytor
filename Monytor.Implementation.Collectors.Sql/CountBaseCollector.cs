
using FluentValidation;
using Monytor.Core.Configurations;

namespace Monytor.Implementation.Collectors.Sql {
    public abstract class CountBaseCollector : Collector {
        private static readonly CountCollectorValidator Validator = new CountCollectorValidator();

        public string TableName { get; set; }
        public string WhereClause { get; set; }
        public string ConnectionString { get; set; }
        public override string GroupName { get; set; } = "Count";
        
        public override void ValidateAndThrow() {
            base.ValidateAndThrow();
            Validator.ValidateAndThrow(this);
        }
    }
}