namespace TripFinder.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using TripFinder.Data.Models;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Trips;

    [Authorize]
    public class TripsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITripsService tripsService;
        private readonly ICarsService carsService;
        private readonly IConfiguration configuration;

        private readonly string imagePathPrefix;
        private readonly string cloudinaryPrefix = "https://res.cloudinary.com/{0}/image/upload/";
        private readonly string imageSizing = "w_300,h_300,c_pad,b_black/";

        public TripsController(
            UserManager<ApplicationUser> userManager,
            ITripsService tripsService,
            ICarsService carsService,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.tripsService = tripsService;
            this.carsService = carsService;
            this.configuration = configuration;
            this.imagePathPrefix = string.Format(this.cloudinaryPrefix, this.configuration["Cloudinary:AppName"]);
        }

        public IActionResult All()
        {
            return this.View();
        }

        public async Task<IActionResult> Create()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var car = this.carsService.GetById(user.CarId);

            if (car == null)
            {
                return this.RedirectToAction("Index", "Cars");
            }

            this.ViewBag.TotalSeats = car.PassengerSeats;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TripCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var user = await this.userManager
                .Users
                .Include(u => u.Car)
                .SingleAsync(u => u.Email == this.User.Identity.Name);

            if (user.CarId == null)
            {
                return this.RedirectToAction("Index", "Cars");
            }

            var tripId = await this.tripsService.CreateAsync(inputModel, user);

            if (tripId == null)
            {
                return this.View();
            }

            return this.RedirectToAction("Details", new { id = tripId });
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.tripsService.GetById<TripDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/");
            }

            viewModel.Driver.AvatarImage.Url = viewModel.Driver.AvatarImage.Url == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.Driver.AvatarImage.Url;

            viewModel.Car.Image.Url = viewModel.Car.Image.Url == null
                ? "/img/car-avatar.png"
                : this.imagePathPrefix + this.imageSizing + viewModel.Car.Image.Url;

            return this.View(viewModel);
        }

        public IActionResult Search()
        {
            return this.View();
        }
    }
}
