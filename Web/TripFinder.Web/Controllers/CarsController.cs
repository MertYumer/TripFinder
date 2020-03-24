namespace TripFinder.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TripFinder.Data.Models;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Cars;

    [Authorize]
    public class CarsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICarsService carsService;

        public CarsController(UserManager<ApplicationUser> userManager, ICarsService carsService)
        {
            this.userManager = userManager;
            this.carsService = carsService;
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
            var carProfileViewModel = this.carsService.GetById<CarDetailsViewModel>(id);

            return this.View(carProfileViewModel);
        }
    }
}
