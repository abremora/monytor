using Monytor.Core.Configurations;
using System;

namespace Monytor.Core.Models {
    // ToDo: Maybe we can just use CollectorConfig as soon as config by file is gone.
    public class CollectorConfigStored : CollectorConfig {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string SchedulerAgentId { get; set; }

        public static string CreateId() {
            return nameof(CollectorConfigStored) + "/" + Guid.NewGuid();
        }
    }
}