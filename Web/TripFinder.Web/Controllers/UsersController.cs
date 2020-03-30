namespace TripFinder.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Users;

    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IConfiguration configuration;

        private readonly string imagePathPrefix;
        private readonly string cloudinaryPrefix = "https://res.cloudinary.com/{0}/image/upload/";
        private readonly string imageSizing = "w_300,h_300,c_fill/";

        public UsersController(IUsersService usersService, IConfiguration configuration)
        {
            this.usersService = usersService;
            this.configuration = configuration;
            this.imagePathPrefix = string.Format(this.cloudinaryPrefix, this.configuration["Cloudinary:AppName"]);
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.usersService.GetById<UserDetailsViewModel>(id);

            viewModel.AvatarImageUrl = viewModel.AvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.AvatarImageUrl;

            return this.View(viewModel);
        }

        public IActionResult Edit(string id)
        {
            var viewModel = this.usersService.GetById<UserEditViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/");
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

            return this.RedirectToAction("Details", new { id = userId });
        }
    }
}
