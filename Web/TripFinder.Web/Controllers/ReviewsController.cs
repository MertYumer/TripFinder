namespace TripFinder.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using TripFinder.Data.Models;
    using TripFinder.Services.Data;
    using TripFinder.Web.ViewModels.Reviews;

    public class ReviewsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IReviewsService reviewsService;

        private readonly IConfiguration configuration;
        private readonly string imagePathPrefix;
        private readonly string cloudinaryPrefix = "https://res.cloudinary.com/{0}/image/upload/";
        private readonly string imageSizing = "w_300,h_300,c_fill/";

        public ReviewsController(
            UserManager<ApplicationUser> userManager,
            IReviewsService reviewsService,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.reviewsService = reviewsService;
            this.configuration = configuration;
            this.imagePathPrefix = string.Format(this.cloudinaryPrefix, this.configuration["Cloudinary:AppName"]);
        }

        public async Task<IActionResult> All(string userId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            //if (user.HasUsersToReview)
            //{
            //    return this.RedirectToAction("Pending", new { userId });
            //}

            return this.View();
        }

        public async Task<IActionResult> Pending(string userId)
        {
            var reviewViewModels = await this.reviewsService
                .GetLastTripPassengers<UserReviewViewModel>(userId);

            if (reviewViewModels == null)
            {
                return this.RedirectToAction("All", new { userId });
            }

            if (reviewViewModels.Count() == 0)
            {
                return this.RedirectToAction("All", new { userId });
            }

            foreach (var review in reviewViewModels)
            {
                review.AvatarImageUrl = review.AvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.imageSizing + review.AvatarImageUrl;
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
