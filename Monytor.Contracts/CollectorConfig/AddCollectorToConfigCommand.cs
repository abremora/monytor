namespace Monytor.Contracts.CollectorConfig {
    public abstract class AddCollectorToConfigCommand {
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string GroupName { get; set; }
        public string StartingTimeDelay { get; set; }
        public string RandomTimeDelay { get; set; }
        public string PollingInterval { get; set; }
        public bool OverlappingRecurring { get; set; }
        public string StartingTime { get; set; }
        public string EndAt { get; set; }
        public int Priority { get; set; }
    }
}
