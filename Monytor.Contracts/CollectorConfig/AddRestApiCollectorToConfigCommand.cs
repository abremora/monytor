namespace Monytor.Contracts.CollectorConfig {
    public class AddRestApiCollectorToConfigCommand : AddCollectorToConfigCommand {
        public string RequestUri { get; set; }
        public string JsonPath { get; set; }
        public string TagName { get; set; }
    }
}
