namespace Monytor.Core.Configurations {
    public abstract class NotificationBehaviorBase : Behavior {
        public abstract void Run(Notification notification, string shortDescription, string longDescription);
    }
}
