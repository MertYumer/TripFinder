namespace TripFinder.Web.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using TripFinder.Data.Models;
    using TripFinder.Services.Data;

    public class HomeController : Controller
    {
        private readonly IUsersService usersService;

        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            IUsersService usersService,
            UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user != null)
            {
                var notificationsCount = this.usersService.GetUserNotificationsCount(user.Id);
                this.TempData["notificationsCount"] = notificationsCount;
            }

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }
    }
}
