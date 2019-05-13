namespace Monytor.Contracts.CollectorConfig {
    public class AddRavenDbStartingWithCollectorToConfigCommand : AddRavenDbCollectorToConfigCommand {
        public string StartingWith { get; set; }
    }
}
