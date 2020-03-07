namespace TripFinder.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TripFinder.Services.Data;

    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [Authorize]
        public IActionResult Details(string id)
        {
            var userProfileViewModel = this.usersService.GetUserById(id);

            return this.View(userProfileViewModel);
        }
    }
}