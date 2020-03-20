namespace TripFinder.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TripFinder.Data.Models;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Trips;

    [Authorize]
    public class TripsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITripsService tripsService;

        public TripsController(UserManager<ApplicationUser> userManager, ITripsService tripsService)
        {
            this.userManager = userManager;
            this.tripsService = tripsService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(TripCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user.CarId == null)
            {
                return this.BadRequest("User does not have a car.");
            }

            if (inputModel.FreeSeats > user.Car.PassengerSeats)
            {
                return this.BadRequest("Free seats are more than the available car seats.");
            }

            var tripId = await this.tripsService.CreateAsync(inputModel, user);

            if (tripId == null)
            {
                return this.View();
            }

            return this.Redirect("/Home/Index");
        }

        [Authorize]
        public IActionResult Search()
        {
            return this.View();
        }
    }
}