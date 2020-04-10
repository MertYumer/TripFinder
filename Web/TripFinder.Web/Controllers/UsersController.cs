namespace TripFinder.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using TripFinder.Data.Models;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Notifications;
    using TripFinder.Web.ViewModels.Users;

    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IConfiguration configuration;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        private readonly string imagePathPrefix;
        private readonly string cloudinaryPrefix = "https://res.cloudinary.com/{0}/image/upload/";
        private readonly string imageSizing = "w_300,h_300,c_fill/";

        public UsersController(
            IUsersService usersService,
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.configuration = configuration;
            this.imagePathPrefix = string.Format(this.cloudinaryPrefix, this.configuration["Cloudinary:AppName"]);
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.usersService.GetById<UserDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            viewModel.AvatarImageUrl = viewModel.AvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.AvatarImageUrl;

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = this.usersService.GetById<UserEditViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user.Id != viewModel.Id)
            {
                return this.RedirectToAction("Forbid", "Errors");
            }

            viewModel.AvatarImageUrl = viewModel.AvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.AvatarImageUrl;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Edit", new { id = inputModel.Id });
            }

            var userId = await this.usersService.UpdateAsync(inputModel);

            if (userId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            this.TempData["Notification"] = "Your profile was successfully edited.";

            return this.RedirectToAction("Details", new { id = userId });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var userId = this.usersService.CheckForUserById(id);

            if (userId == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user.Id != userId)
            {
                return this.RedirectToAction("Forbid", "Errors");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string id)
        {
            var userId = await this.usersService.DeleteAsync(id);

            if (userId == null)
            {
                return this.RedirectToAction("Delete", new { id });
            }

            await this.signInManager.SignOutAsync();

            this.TempData["Notification"] = "Your profile was successfully deleted.";

            return this.RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> JoinTrip(string receiverId, string tripId, string senderId)
        {
            var receiver = this.usersService.GetById(receiverId);
            var sender = this.usersService.GetById(senderId);

            if (receiver == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            if (receiver.UserTrips.All(ut => ut.TripId != tripId))
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            var subject = NotificationSubject.RequestJoin;

            var notificationId = await this.usersService.SendNotificationAsync(receiver, sender, tripId, subject);

            if (notificationId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            this.TempData["Notification"] = "You successfully made request for the trip.";

            return this.RedirectToAction("Details", "Trips", new { id = tripId });
        }

        public IActionResult GetNotifications(string userId)
        {
            var notificationsViewModel = this.usersService
                .GetUserNotifications<NotificationViewModel>(userId)
                .ToList();

            var notificationsAllViewModel = new UserNotificationsViewModel
            {
                ReceivedNotifications = notificationsViewModel.Where(x => x.ReceiverId == userId).ToList(),
                SentNotifications = notificationsViewModel.Where(x => x.SenderId == userId).ToList(),
            };

            return this.View("Notifications", notificationsAllViewModel);
        }
    }
}
