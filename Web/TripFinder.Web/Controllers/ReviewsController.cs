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

            var reviewsForUserViewModels = await this.reviewsService
                .GetReviewsForUser<ReviewForUserViewModel>(userId);

            var reviewsByUserViewModels = await this.reviewsService
                .GetReviewsByUser<ReviewByUserViewModel>(userId);

            foreach (var review in reviewsForUserViewModels)
            {
                review.ReviewerAvatarImageUrl = review.ReviewerAvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.imageSizing + review.ReviewerAvatarImageUrl;
            }

            foreach (var review in reviewsByUserViewModels)
            {
                review.ReviewedUserAvatarImageUrl = review.ReviewedUserAvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.imageSizing + review.ReviewedUserAvatarImageUrl;
            }

            var reviewsAllViewModel = new ReviewsAllViewModel
            {
                ReviewsForUser = reviewsForUserViewModels,
                ReviewsByUser = reviewsByUserViewModels,
            };

            return this.View(reviewsAllViewModel);
        }

        public async Task<IActionResult> Pending(string userId)
        {
            var reviewPendingViewModels = await this.reviewsService
                .GetPendingReviews<ReviewPendingViewModel>(userId);

            if (reviewPendingViewModels == null)
            {
                return this.RedirectToAction("All", new { userId });
            }

            if (reviewPendingViewModels.Count() == 0)
            {
                return this.RedirectToAction("All", new { userId });
            }

            foreach (var review in reviewPendingViewModels)
            {
                review.AvatarImageUrl = review.AvatarImageUrl == null
                ? "/img/avatar.png"
                : this.imagePathPrefix + this.imageSizing + review.AvatarImageUrl;
            }

            var reviewsPendingViewModel = new ReviewsPendingViewModel
            {
                Reviews = reviewPendingViewModels,
            };

            return this.View(reviewsPendingViewModel);
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
