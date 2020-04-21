namespace TripFinder.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Cars;

    public class CarsController : AdministrationController
    {
        private readonly ICarsService carsService;

        private readonly IConfiguration configuration;
        private readonly string imagePathPrefix;
        private readonly string cloudinaryPrefix = "https://res.cloudinary.com/{0}/image/upload/";
        private readonly string imageSizing = "w_300,h_300,c_pad,b_black/";

        public CarsController(
            ICarsService carsService,
            IConfiguration configuration)
        {
            this.carsService = carsService;
            this.configuration = configuration;
            this.imagePathPrefix = string.Format(this.cloudinaryPrefix, this.configuration["Cloudinary:AppName"]);
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.carsService.GetDeletedCarDetailsAsync<CarDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            viewModel.ImageUrl = viewModel.ImageUrl == null
                ? "/img/car-avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.ImageUrl;

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var viewModel = await this.carsService.GetByIdAsync<CarDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            viewModel.ImageUrl = viewModel.ImageUrl == null
                ? "/img/car-avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.ImageUrl;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string id)
        {
            var car = await this.carsService.GetByIdAsync(id);

            if (car == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            var carId = await this.carsService.DeleteAsync(id);

            if (carId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            this.TempData["Notification"] = "You successfully deleted car from TripFinder.";

            return this.RedirectToAction("AllCars", "Dashboard");
        }
    }
}
