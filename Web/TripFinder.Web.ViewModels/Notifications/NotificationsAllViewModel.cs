namespace TripFinder.Web.ViewModels.Notifications
{
    using System.Collections.Generic;

    public class NotificationsAllViewModel
    {
        public NotificationsAllViewModel()
        {
            this.ReceivedNotifications = new HashSet<NotificationViewModel>();
            this.SentNotifications = new HashSet<NotificationViewModel>();
        }

        public IEnumerable<NotificationViewModel> ReceivedNotifications { get; set; }

        public IEnumerable<NotificationViewModel> SentNotifications { get; set; }
    }
}
