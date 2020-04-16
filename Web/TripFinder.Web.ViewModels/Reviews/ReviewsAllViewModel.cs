namespace TripFinder.Web.ViewModels.Reviews
{
    using System.Collections.Generic;

    public class ReviewsAllViewModel
    {
        public ReviewsAllViewModel()
        {
            this.Reviews = new HashSet<UserReviewViewModel>();
        }

        public IEnumerable<UserReviewViewModel> Reviews { get; set; }
    }
}
