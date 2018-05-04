using Monytor.Core.Configurations;

namespace Monytor.Implementation.Notifications {
    public class EmailNotification : Notification {
        public override string Id => "Monytor.EmailNotification";

        public string Smtp { get; set; }
        public int Port { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; } = true;
        public string[] To { get; set; }
        public string From { get; set; }
        public string SubjectPrefix { get; set; }
    }
}