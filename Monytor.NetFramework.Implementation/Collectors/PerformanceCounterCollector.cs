using Monytor.Core.Configurations;

namespace Monytor.NetFramework.Implementation {
    public class PerformanceCounterCollector : Collector {
        public override string GroupName { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Instance { get; set; }

        public PerformanceCounterCollector() {
            GroupName = "PerformanceCounterCollector";
        }
    }
}
