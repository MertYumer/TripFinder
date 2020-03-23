namespace TripFinder.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TripFinder.Data.Models;

    [Authorize]
    public class CarsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public CarsController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Create()
        //{ 
        
        //}
    }
}
