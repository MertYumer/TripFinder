namespace TripFinder.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Administration.Dashboard;

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

            var statisticsViewModel = new StatisticsViewModel
            {
                AllUsersCount = allUsersCount,
                ActiveUsersCount = activeUsersCount,
                DeletedUsersCount = deletedUsersCount,
                AllCarsCount = allCarsCount,
                CurrentCarsCount = currentCarsUsersCount,
                DeletedCarsCount = deletedCarsCount,
                AllTripsCount = allTripsCount,
                ActiveTripsCount = activeTripsCount,
                DeletedTripsCount = deletedTripsCount,
            };

            return this.View(statisticsViewModel);
        }

        public async Task<IActionResult> AllUsers()
        {
            var userViewModels = await this.usersService.GetAllUsersAsync<UserViewModel>();

            var usersAllViewModel = new UsersAllViewModel
            {
                Users = userViewModels,
            };

            return this.View(usersAllViewModel);
        }
    }
}
