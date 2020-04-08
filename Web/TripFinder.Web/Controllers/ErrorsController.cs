namespace TripFinder.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorsController : Controller
    {
        public new IActionResult NotFound()
        {
            return this.View();
        }

        public new IActionResult BadRequest()
        {
            return this.View();
        }

        public new IActionResult Forbid()
        {
            return this.View();
        }
    }
}
