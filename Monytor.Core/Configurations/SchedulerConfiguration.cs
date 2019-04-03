using System;

namespace Monytor.Core.Configurations {
    public class SchedulerConfiguration {
        public string SchedulerAgentId { get; set; }
        public CollectorConfigProvider CollectorConfigProvider { get; set; }
        public string CollectorConfigFileName { get; set; }
        public TimeSpan CollectorPollingInterval { get; set; }
        public StorageProvider StorageProvider { get; set; }
        public string StorageProviderConnectionString { get; set; }
    }
}