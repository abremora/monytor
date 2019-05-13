namespace Monytor.Contracts.CollectorConfig {
    public abstract class AddRavenDbCollectorToConfigCommand : AddCollectorToConfigCommand {
        public string DatabaseUrl { get; set; }
        public string DatabaseName { get; set; }
    }
}
