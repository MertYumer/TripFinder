namespace TripFinder.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
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
        private readonly string imageSizing = "w_300,h_300,c_crop,g_face,r_max/w_300/";

        public CarsController(UserManager<ApplicationUser> userManager, ICarsService carsService, IConfiguration configuration)
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

            viewModel.ImageUrl = viewModel.ImageUrl == null
                ? "/img/car-avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.ImageUrl;

            return this.View(viewModel);
        }

        public IActionResult Edit(string id)
        {
            var viewModel = this.carsService.GetById<CarDetailsViewModel>(id);

            if (viewModel.ImageUrl != null)
            {
                viewModel.ImageUrl = this.imagePathPrefix + this.imageSizing + viewModel.ImageUrl;
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CarEditInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.carsService.UpdateAsync(inputModel);

            return this.RedirectToAction("Details", new { id = inputModel.Id });
        }
    }
}
