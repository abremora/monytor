using System.Collections.Generic;

namespace Monytor.Core.Configurations {
    public class CollectorConfig {
        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public List<Collector> Collectors { get; set; } = new List<Collector>();
    }
}