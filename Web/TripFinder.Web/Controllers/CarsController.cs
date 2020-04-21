namespace TripFinder.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using TripFinder.Common;
    using TripFinder.Data.Models;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Cars;

    [Authorize]
    public class CarsController : Controller
    {
        private readonly ICarsService carsService;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IConfiguration configuration;
        private readonly string imagePathPrefix;
        private readonly string cloudinaryPrefix = "https://res.cloudinary.com/{0}/image/upload/";
        private readonly string imageSizing = "w_300,h_300,c_pad,b_black/";

        public CarsController(
            UserManager<ApplicationUser> userManager,
            ICarsService carsService,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.carsService = carsService;
            this.configuration = configuration;
            this.imagePathPrefix = string.Format(this.cloudinaryPrefix, this.configuration["Cloudinary:AppName"]);
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Create()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user.CarId != null)
            {
                return this.RedirectToAction("Index", "Cars");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var carId = await this.carsService.CreateAsync(inputModel, user);

            if (carId == null)
            {
                return this.View();
            }

            this.TempData["Notification"] = "Car was successfully created.";

            return this.RedirectToAction("Details", new { id = carId });
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = this.carsService.GetById<CarDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var isAdmin = await this.userManager.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);

            if (user.CarId != viewModel.Id && !isAdmin)
            {
                return this.RedirectToAction("Forbid", "Errors");
            }

            viewModel.ImageUrl = viewModel.ImageUrl == null
                ? "/img/car-avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.ImageUrl;

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = this.carsService.GetById<CarDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user.CarId != viewModel.Id)
            {
                return this.RedirectToAction("Forbid", "Errors");
            }

            viewModel.ImageUrl = viewModel.ImageUrl == null
                ? "/img/car-avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.ImageUrl;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CarEditInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Edit", new { id = inputModel.Id });
            }

            var carId = await this.carsService.UpdateAsync(inputModel);

            if (carId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            this.TempData["Notification"] = "Car was successfully edited.";

            return this.RedirectToAction("Details", new { id = carId });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var viewModel = this.carsService.GetById<CarDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user.CarId != viewModel.Id)
            {
                return this.RedirectToAction("Forbid", "Errors");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string id)
        {
            var carId = await this.carsService.DeleteAsync(id);

            if (carId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            this.TempData["Notification"] = "Car was successfully deleted.";

            return this.RedirectToAction("Index");
        }
    }
}
