using System;
using System.Collections.Generic;
using System.Text;

namespace Monytor.Contracts.CollectorConfig {
    public class CreateCollectorConfigCommand {
        public string DisplayName { get; set; }
        public string SchedulerAgentId { get; set; }
    }
}
