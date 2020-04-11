namespace TripFinder.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Notifications;
    using TripFinder.Web.ViewModels.Users;

    public class NotificationsController : Controller
    {
        private readonly ITripsService tripsService;
        private readonly INotificationsService notificationsService;

        public NotificationsController(
            ITripsService tripsService,
            INotificationsService notificationsService)
        {
            this.tripsService = tripsService;
            this.notificationsService = notificationsService;
        }

        public IActionResult AllNotifications(string userId)
        {
            var notificationsViewModel = this.notificationsService
                .GetUserNotifications<NotificationViewModel>(userId)
                .ToList();

            var notificationsAllViewModel = new NotificationsAllViewModel
            {
                ReceivedNotifications = notificationsViewModel.Where(x => x.ReceiverId == userId).ToList(),
                SentNotifications = notificationsViewModel.Where(x => x.SenderId == userId).ToList(),
            };

            return this.View("Notifications", notificationsAllViewModel);
        }

        public async Task<IActionResult> AcceptTripRequest(string notificationId)
        {
            var notification = this.notificationsService.GetById(notificationId);

            if (notification == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            var trip = this.tripsService.GetById(notification.TripId);

            var tripId = await this.tripsService.AddUserToTripAsync(notification.SenderId, notification.ReceiverId, trip);

            if (tripId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            this.TempData["Notification"] = "You successfully accepted request for the trip.";

            return this.RedirectToAction("Details", "Trips", new { id = tripId });
        }
    }
}