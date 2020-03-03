namespace TripFinder.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        public IActionResult Login()
        {
            return this.View();
        }

        public IActionResult Register()
        {
            return this.View();
        }

        public IActionResult Profile()
        {
            return this.View();
        }
    }
}
