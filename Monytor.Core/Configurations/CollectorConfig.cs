using System.Collections.Generic;

namespace Monytor.Core.Configurations {
    public class CollectorConfig {
        public List<Notification> Notifications { get; set; }
        public List<Collector> Collectors { get; set; }       
    }
}