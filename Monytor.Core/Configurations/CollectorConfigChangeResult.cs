using System.Collections.Generic;
using System.Linq;

namespace Monytor.Core.Configurations {
    public class CollectorConfigChangeResult {
        public List<Notification> AddedNotifications { get; } = new List<Notification>();
        public List<Notification> RemovedNotifications { get; } = new List<Notification>();
        public List<Notification> ChangedNotifications { get; } = new List<Notification>();

        public List<Collector> AddedCollectors { get; } = new List<Collector>();
        public List<Collector> RemovedCollectors { get; } = new List<Collector>();
        public List<Collector> ChangedCollectors { get; } = new List<Collector>();

        public bool HasChanges =>
            AddedCollectors.Any() || RemovedCollectors.Any() || ChangedCollectors.Any()
            || AddedNotifications.Any() || RemovedNotifications.Any() || ChangedNotifications.Any();
    }
}