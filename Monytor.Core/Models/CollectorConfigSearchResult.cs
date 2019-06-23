using System;
using System.Collections.Generic;
using System.Text;

namespace Monytor.Core.Models {
    public class CollectorConfigSearchResult {
        public string CollectorConfigId { get; set; }
        public string DisplayName { get; set; }
        public string SchedulerAgentId { get; set; }
        public int CollectorCount { get; set; }
        public int NotificationCount { get; set; }
    }
}
