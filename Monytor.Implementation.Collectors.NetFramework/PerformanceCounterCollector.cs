using FluentValidation;
using Monytor.Core.Configurations;
using Monytor.Implementation.Collectors.Sql;

namespace Monytor.Implementation.Collectors.NetFramework {
    public class PerformanceCounterCollector : Collector {
        private static readonly PerformanceCounterCollectorValidator Validator = new PerformanceCounterCollectorValidator();

        public override string GroupName { get; set; } = "PerformanceCounterCollector";
        public string Category { get; set; }
        public string Counter { get; set; }
        public string Instance { get; set; }
        public string MachineName { get; set; }

        public override void ValidateAndThrow() {
            base.ValidateAndThrow();
            Validator.ValidateAndThrow(this);
        }
    }
}
