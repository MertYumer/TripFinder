namespace TripFinder.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using TripFinder.Web.ViewModels.Notifications;

    public class UserNotificationsViewModel
    {
        public UserNotificationsViewModel()
        {
            this.ReceivedNotifications = new HashSet<NotificationViewModel>();
            this.SentNotifications = new HashSet<NotificationViewModel>();
        }

        public IEnumerable<NotificationViewModel> ReceivedNotifications { get; set; }

        public IEnumerable<NotificationViewModel> SentNotifications { get; set; }
    }
}
