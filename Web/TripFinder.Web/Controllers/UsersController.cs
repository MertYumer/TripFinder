namespace TripFinder.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using TripFinder.Data.Models;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Users;

    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IConfiguration configuration;
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
    }
}
