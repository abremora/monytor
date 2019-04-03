namespace Monytor.Contracts.CollectorConfig {
    public class AddPerformanceCounterCollectorToConfigCommand : AddCollectorToConfigCommand {
        public string Category { get; set; }
        public string Counter { get; set; }
        public string Instance { get; set; }
        public string MachineName { get; set; }
    }
}
