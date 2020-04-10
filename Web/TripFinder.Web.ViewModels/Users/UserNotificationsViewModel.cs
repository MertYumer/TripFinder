namespace TripFinder.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using TripFinder.Web.ViewModels.Notifications;

    public class UserNotificationsViewModel
    {
        public UserNotificationsViewModel()
        {
            this.ReceivedNotifications = new HashSet<NotificationViewModel>();
        }

        public ICollection<NotificationViewModel> ReceivedNotifications { get; set; }
    }
}
