using Monytor.Core.Configurations;

namespace Monytor.NetFramework.Implementation {
    public class PerformanceCounterCollector : Collector {
        public override string GroupName { get; set; }
        public string Category { get; set; }
        public string Counter { get; set; }
        public string Instance { get; set; }
        public string MachineName { get; set; }

        public PerformanceCounterCollector() {
            GroupName = "PerformanceCounterCollector";
        }
    }
}
