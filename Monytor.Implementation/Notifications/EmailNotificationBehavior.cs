using Monytor.Core.Configurations;
using System.Net;
using System.Net.Mail;

namespace Monytor.Implementation.Notifications {
    public class EmailNotificationBehavior : NotificationBehavior<EmailNotification> {
        public override void Run(Notification notification, string shortDescription, string longDescription) {
            var typedNotification = notification as EmailNotification;
            if (typedNotification == null) return;

            SmtpClient client = new SmtpClient(typedNotification.Smtp, typedNotification.Port) {
                EnableSsl = typedNotification.EnableSsl,
                UseDefaultCredentials = typedNotification.UseDefaultCredentials,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(typedNotification.User, typedNotification.Password),
            };

            MailMessage mailMessage = new MailMessage {
                From = new MailAddress(typedNotification.From),
                Body = longDescription,
                Subject = typedNotification.SubjectPrefix + shortDescription
            };

            foreach (var to in typedNotification.To) {
                mailMessage.To.Add(to);
            }

            client.Send(mailMessage);
        }
    }
}
