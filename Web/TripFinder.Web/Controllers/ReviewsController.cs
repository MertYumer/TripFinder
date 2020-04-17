namespace TripFinder.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using TripFinder.Data.Models;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Reviews;

    public class ReviewsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IReviewsService reviewsService;

        public ReviewsController(
            UserManager<ApplicationUser> userManager,
            IReviewsService reviewsService)
        {
            this.userManager = userManager;
            this.reviewsService = reviewsService;
        }

        public IActionResult All()
        {
            return this.View();
        }

        public async Task<IActionResult> Pending()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var reviewViewModels = this.reviewsService
                .GetLastTripPassengers<UserReviewViewModel>(user.Id)
                .ToList();

            if (reviewViewModels == null)
            {
                this.RedirectToAction("All");
            }

            if (reviewViewModels.Count == 0)
            {
                this.RedirectToAction("All");
            }

            var reviewsViewModel = new ReviewsAllViewModel
            {
                Reviews = reviewViewModels,
            };

            return this.View(reviewsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Pending(IFormCollection data)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var userGaveRatings = this.reviewsService.AddReviews(data, user.Id).Result;

            if (!userGaveRatings)
            {
                return this.RedirectToAction("BadRequest", "Errors");
            }

            this.TempData["Notification"] = "You successfully gave ratings for the users.";

            return this.RedirectToAction("Index", "Home");
        }
    }
}
