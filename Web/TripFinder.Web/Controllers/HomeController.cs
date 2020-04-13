namespace TripFinder.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }
    }
}
