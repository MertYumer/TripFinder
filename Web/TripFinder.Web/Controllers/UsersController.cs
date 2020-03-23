namespace TripFinder.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Users;

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
            var userProfileViewModel = this.usersService.GetById<UserDetailsViewModel>(id);

            return this.View(userProfileViewModel);
        }
    }
}
