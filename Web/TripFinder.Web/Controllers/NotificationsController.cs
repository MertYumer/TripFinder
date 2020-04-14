namespace TripFinder.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TripFinder.Data.Models;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Notifications;

    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly ITripsService tripsService;
        private readonly INotificationsService notificationsService;

        private readonly UserManager<ApplicationUser> userManager;

        public NotificationsController(
            ITripsService tripsService,
            INotificationsService notificationsService,
            UserManager<ApplicationUser> userManager)
        {
            this.tripsService = tripsService;
            this.notificationsService = notificationsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(string userId)
        {
            var notificationsViewModel = await this.notificationsService
                .GetUserNotifications<NotificationViewModel>(userId);

            var notificationsAllViewModel = new NotificationsAllViewModel
            {
                ReceivedNotifications = notificationsViewModel.Where(x => x.ReceiverId == userId).ToList(),
                SentNotifications = notificationsViewModel.Where(x => x.SenderId == userId).ToList(),
            };

            return this.View("Notifications", notificationsAllViewModel);
        }

        public async Task<IActionResult> SendTripNotification(string receiverId, string tripId, string senderId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (senderId != user.Id)
            {
                return this.RedirectToAction("Forbid", "Errors");
            }

            var subject = NotificationSubject.RequestJoin;

            var notificationId = await this.notificationsService.SendNotificationAsync(receiverId, senderId, tripId, subject);

            if (notificationId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            this.TempData["Notification"] = "You successfully made request for the trip.";

            return this.RedirectToAction("Details", "Trips", new { id = tripId });
        }

        public async Task<IActionResult> CanselTripRequest(string notificationId)
        {
            var notification = await this.DeleteRequest(notificationId);

            if (notification == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            var subject = NotificationSubject.CancelJoin;

            var newNotificationId = await this.notificationsService
                .SendNotificationAsync(notification.ReceiverId, notification.SenderId, notification.TripId, subject);

            if (newNotificationId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            this.TempData["Notification"] = "You successfully deleted request for the trip.";

            return this.RedirectToAction("All", new { userId = user.Id });
        }

        public async Task<IActionResult> AcceptTripRequest(string notificationId)
        {
            var notification = this.notificationsService.GetById(notificationId);

            if (notification == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            var tripId = await this.tripsService.AddUserToTripAsync(notification.SenderId, notification.ReceiverId, notification.TripId);

            if (tripId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            await this.notificationsService.DeleteAsync(notification.Id);

            var subject = NotificationSubject.AcceptRequest;

            var newNotificationId = await this.notificationsService
                .SendNotificationAsync(notification.SenderId, notification.ReceiverId, tripId, subject);

            if (newNotificationId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            this.TempData["Notification"] = "You successfully accepted request for the trip.";

            return this.RedirectToAction("Details", "Trips", new { id = tripId });
        }

        public async Task<IActionResult> RejectTripRequest(string notificationId)
        {
            var notification = await this.DeleteRequest(notificationId);

            if (notification == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            var subject = NotificationSubject.RejectRequest;

            var newNotificationId = await this.notificationsService
                .SendNotificationAsync(notification.SenderId, notification.ReceiverId, notification.TripId, subject);

            if (newNotificationId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            this.TempData["Notification"] = "You successfully rejected request for the trip.";

            return this.RedirectToAction("All", new { userId = user.Id });
        }

        private async Task<Notification> DeleteRequest(string notificationId)
        {
            var notification = this.notificationsService.GetById(notificationId);

            var id = await this.notificationsService.DeleteAsync(notificationId);

            if (id == null)
            {
                return null;
            }

            return notification;
        }
    }
}
