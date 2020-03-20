namespace TripFinder.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class TripsController : Controller
    {
        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult Search()
        {
            return this.View();
        }
    }
}