namespace TripFinder.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using TripFinder.Data.Models;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Cars;

    [Authorize]
    public class CarsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICarsService carsService;
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

        public IActionResult Create()
        {
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

            return this.RedirectToAction("Details", new { id = carId });
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.carsService.GetById<CarDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/");
            }

            viewModel.ImageUrl = viewModel.ImageUrl == null
                ? "/img/car-avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.ImageUrl;

            return this.View(viewModel);
        }

        public IActionResult Edit(string id)
        {
            var viewModel = this.carsService.GetById<CarDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/");
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
                return this.View(inputModel.Id);
            }

            var carId = await this.carsService.UpdateAsync(inputModel);

            if (carId == null)
            {
                return this.RedirectToAction("Edit", new { id = inputModel.Id });
            }

            return this.RedirectToAction("Details", new { id = carId });
        }

        public IActionResult Delete(string id)
        {
            var viewModel = this.carsService.GetById<CarDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string id)
        {
            var carId = await this.carsService.DeleteAsync(id);

            if (carId == null)
            {
                return this.RedirectToAction("Delete", new { id });
            }

            return this.RedirectToAction("Index");
        }
    }
}
