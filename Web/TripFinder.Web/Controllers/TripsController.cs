namespace TripFinder.Web.Controllers
{
    using System;
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
        private const int TripsPerPage = 6;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly ITripsService tripsService;
        private readonly ICarsService carsService;

        private readonly IConfiguration configuration;
        private readonly string imagePathPrefix;
        private readonly string cloudinaryPrefix = "https://res.cloudinary.com/{0}/image/upload/";
        private readonly string driverImageSizing = "w_300,h_300,c_fill/";
        private readonly string carImageSizing = "w_300,h_300,c_pad,b_black/";

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

        public async Task<IActionResult> All(int page = 1)
        {
            var tripsViewModel = await this.tripsService
                .GetAllTrips<TripViewModel>(TripsPerPage, (page - 1) * TripsPerPage);

            foreach (var trip in tripsViewModel)
            {
                trip.DriverAvatarImageUrl = trip.DriverAvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.driverImageSizing + trip.DriverAvatarImageUrl;
            }

            var allTripsCount = this.tripsService.GetAllTripsCount();

            var tripsAllViewModel = new TripsViewModel
            {
                Title = "Last added trips:",
                CurrentPage = page,
                PagesCount = (int)Math.Ceiling((double)allTripsCount / TripsPerPage),
                Trips = tripsViewModel,
            };

            if (tripsAllViewModel.PagesCount == 0)
            {
                tripsAllViewModel.PagesCount = 1;
            }

            return this.View(tripsAllViewModel);
        }

        public async Task<IActionResult> MyTrips(string userId, int page = 1)
        {
            var tripsViewModel = await this.tripsService
                .GetMyTrips<TripViewModel>(userId, TripsPerPage, (page - 1) * TripsPerPage);

            foreach (var trip in tripsViewModel)
            {
                trip.DriverAvatarImageUrl = trip.DriverAvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.driverImageSizing + trip.DriverAvatarImageUrl;
            }

            var myTripsCount = this.tripsService.GetMyTripsCount(userId);

            var tripsMyViewModel = new TripsViewModel
            {
                Title = "My trips:",
                CurrentPage = page,
                PagesCount = (int)Math.Ceiling((double)myTripsCount / TripsPerPage),
                Trips = tripsViewModel,
            };

            if (tripsMyViewModel.PagesCount == 0)
            {
                tripsMyViewModel.PagesCount = 1;
            }

            return this.View("All", tripsMyViewModel);
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

            if (inputModel.DateOfDeparture.Date.CompareTo(DateTime.Now.Date) == 0 &&
                inputModel.TimeOfDeparture.TimeOfDay.TotalMinutes < DateTime.Now.TimeOfDay.TotalMinutes)
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
                return this.RedirectToAction("BadRequest", "Errors");
            }

            this.TempData["Notification"] = "Trip was successfully created.";

            return this.RedirectToAction("Details", new { id = tripId });
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = this.tripsService.GetById<TripDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user.Id != viewModel.Driver.Id)
            {
                await this.tripsService.UpdateTripViewsCountAsync(id);
            }

            viewModel.DriverAvatarImageUrl = viewModel.DriverAvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.driverImageSizing + viewModel.DriverAvatarImageUrl;

            viewModel.CarImageUrl = viewModel.CarImageUrl == null
                ? "/img/car-avatar.png"
                : this.imagePathPrefix + this.carImageSizing + viewModel.CarImageUrl;

            foreach (var passenger in viewModel.Passengers)
            {
                passenger.AvatarImageUrl = passenger.AvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.driverImageSizing + passenger.AvatarImageUrl;
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = this.tripsService.GetById<TripEditViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user.Id != viewModel.Driver.Id)
            {
                return this.RedirectToAction("Forbid", "Errors");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TripEditInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Edit", new { id = inputModel.Id });
            }

            var tripId = await this.tripsService.UpdateAsync(inputModel);

            if (tripId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            this.TempData["Notification"] = "Trip was successfully edited.";

            return this.RedirectToAction("Details", new { id = tripId });
        }

        public async Task<IActionResult> Delete(string id)
        {
            var viewModel = this.tripsService.GetById<TripDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
            }

            var user = await this.userManager.GetUserAsync(this.User);

            if (user.Id != viewModel.DriverId)
            {
                return this.RedirectToAction("Forbid", "Errors");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(string id)
        {
            var tripId = await this.tripsService.DeleteAsync(id);

            if (tripId == null)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            this.TempData["Notification"] = "Trip was successfully deleted.";

            return this.RedirectToAction("All");
        }

        public IActionResult Search()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(TripSearchInputModel inputModel, int page = 1)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this.userManager.GetUserId(this.User);

            var tripsViewModel = await this.tripsService
                .ShowSearchResults<TripViewModel>(inputModel, userId, TripsPerPage, (page - 1) * TripsPerPage);

            foreach (var trip in tripsViewModel)
            {
                trip.DriverAvatarImageUrl = trip.DriverAvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.driverImageSizing + trip.DriverAvatarImageUrl;
            }

            var searchResultsCount = this.tripsService.GetSearchResultsCount(inputModel, userId);

            var tripsSearchViewModel = new TripsViewModel
            {
                Title = "Search results:",
                CurrentPage = page,
                PagesCount = (int)Math.Ceiling((double)(searchResultsCount / TripsPerPage)),
                Trips = tripsViewModel,
            };

            if (tripsSearchViewModel.PagesCount == 0)
            {
                tripsSearchViewModel.PagesCount = 1;
            }

            return this.View("All", tripsSearchViewModel);
        }
    }
}
