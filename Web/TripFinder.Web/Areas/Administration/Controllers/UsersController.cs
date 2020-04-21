namespace TripFinder.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Users;

    public class UsersController : AdministrationController
    {
        private readonly IUsersService usersService;

        private readonly IConfiguration configuration;
        private readonly string imagePathPrefix;
        private readonly string cloudinaryPrefix = "https://res.cloudinary.com/{0}/image/upload/";
        private readonly string imageSizing = "w_300,h_300,c_fill/";

        public UsersController(
            IUsersService usersService,
            IConfiguration configuration)
        {
            this.usersService = usersService;
            this.configuration = configuration;
            this.imagePathPrefix = string.Format(this.cloudinaryPrefix, this.configuration["Cloudinary:AppName"]);
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.usersService.GetDeletedUserDetailsAsync<UserDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            viewModel.AvatarImageUrl = viewModel.AvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.AvatarImageUrl;

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var viewModel = await this.usersService.GetById<UserDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            viewModel.AvatarImageUrl = viewModel.AvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.AvatarImageUrl;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string id)
        {
            var userId = await this.usersService.CheckForUserByIdAsync(id);

            if (userId == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            userId = await this.usersService.DeleteAsync(id);

            if (userId == null)
            {
                return this.RedirectToAction("Delete", new { id });
            }

            this.TempData["Notification"] = $"You successfully deleted user's profile.";

            return this.RedirectToAction("AllUsers", "Dashboard");
        }
    }
}
