namespace TripFinder.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Trips;

    public class TripsController : AdministrationController
    {
        private readonly ITripsService tripsService;

        private readonly IConfiguration configuration;
        private readonly string imagePathPrefix;
        private readonly string cloudinaryPrefix = "https://res.cloudinary.com/{0}/image/upload/";
        private readonly string driverImageSizing = "w_300,h_300,c_fill/";
        private readonly string carImageSizing = "w_300,h_300,c_pad,b_black/";

        public TripsController(
            ITripsService tripsService,
            IConfiguration configuration)
        {
            this.tripsService = tripsService;
            this.configuration = configuration;
            this.imagePathPrefix = string.Format(this.cloudinaryPrefix, this.configuration["Cloudinary:AppName"]);
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.tripsService.GetDeletedTripDetailsAsync<TripDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.RedirectToAction("NotFound", "Errors");
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
    }
}
