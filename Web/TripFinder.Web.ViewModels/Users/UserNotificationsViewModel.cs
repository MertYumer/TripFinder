namespace TripFinder.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using TripFinder.Web.ViewModels.Notifications;

    public class UserNotificationsViewModel
    {
        public UserNotificationsViewModel()
        {
            this.Notifications = new HashSet<NotificationViewModel>();
        }

        public ICollection<NotificationViewModel> Notifications { get; set; }
    }
}
