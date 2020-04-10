namespace TripFinder.Web.ViewModels.Notifications
{
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class NotificationViewModel : IMapFrom<Notification>
    {
        public string Id { get; set; }

        public string ReceiverId { get; set; }

        public string ReceiverFirstName { get; set; }

        public string ReceiverLastName { get; set; }

        public string SenderId { get; set; }

        public string SenderFirstName { get; set; }

        public string SenderLastName { get; set; }

        public string TripId { get; set; }

        public string Subject { get; set; }
    }
}
