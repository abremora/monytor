namespace Monytor.Contracts.CollectorConfig {
    public class AddRavenDbCollectionCollectorToConfigCommand : AddRavenDbCollectorToConfigCommand {
        public string CollectionName { get; set; }
    }
}
