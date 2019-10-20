namespace Monytor.Contracts.CollectorConfig
{
    public class EditCollectorConfigCommand
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string SchedulerAgentId { get; set; }
    }
}
