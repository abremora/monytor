using System;

namespace Monytor.Core.Configurations {
    public abstract class Notification {
        public string Id { get; set; }

        public string CreateId() {
            return $"{GetType().Name}/{Guid.NewGuid()}";
        }

        public static string CreateId<TNotification>() {
            return $"{typeof(TNotification).Name}/{Guid.NewGuid()}";
        }
    }
}
