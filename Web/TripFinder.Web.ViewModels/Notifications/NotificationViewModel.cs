namespace TripFinder.Web.ViewModels.Notifications
{
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class NotificationViewModel : IMapFrom<Notification>
    {
        public string Id { get; set; }

        public string ReceiverId { get; set; }

        public string SenderId { get; set; }

        public string TripId { get; set; }

        public NotificationSubject Subject { get; set; }

        public string ReceiverMessage { get; set; }

        public string SenderMessage { get; set; }
    }
}
