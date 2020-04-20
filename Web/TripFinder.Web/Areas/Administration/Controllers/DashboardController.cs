namespace TripFinder.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TripFinder.Services.Data;

    public class DashboardController : AdministrationController
    {
        private readonly IUsersService usersService;
        private readonly ICarsService carsService;
        private readonly ITripsService tripsService;

        public DashboardController(
            IUsersService usersService,
            ICarsService carsService,
            ITripsService tripsService)
        {
            this.usersService = usersService;
            this.carsService = carsService;
            this.tripsService = tripsService;
        }

        public async Task<IActionResult> Index()
        {
            var allUsersCount = await this.usersService.GetAllUsersCountAsync();
            var activeUsersCount = await this.usersService.GetActiveUsersCountAsync();
            var deletedUsersCount = await this.usersService.GetDeletedUsersCountAsync();

            var allCarsCount = await this.carsService.GetAllCarsCountAsync();
            var currentCarsUsersCount = await this.carsService.GetCurrentCarsCountAsync();
            var deletedCarsCount = await this.carsService.GetDeletedCarsCountAsync();

            var allTripsCount = await this.tripsService.GetAllTripsCountWithDeleted();
            var activeTripsCount = await this.tripsService.GetActiveTripsCountAsync();
            var deletedTripsCount = await this.tripsService.GetDeletedTripsCountAsync();

            return this.View();
        }
    }
}
