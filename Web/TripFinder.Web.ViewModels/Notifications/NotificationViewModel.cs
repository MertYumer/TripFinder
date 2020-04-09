namespace TripFinder.Web.ViewModels.Notifications
{
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class NotificationViewModel : IMapFrom<Notification>
    {
        public string Subject { get; set; }
    }
}
