using Monytor.Core.Configurations;
using System;

namespace Monytor.Core.Models {
    public class CollectorConfigStored : CollectorConfig {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string SchedulerAgent { get; set; }

        public static string CreateId() {
            return nameof(CollectorConfigStored) + "/" + Guid.NewGuid();
        }

    }
}