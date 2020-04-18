namespace TripFinder.Web.ViewModels.Reviews
{
    using System.Collections.Generic;

    public class ReviewsAllViewModel
    {
        public ReviewsAllViewModel()
        {
            this.ReviewsForUser = new HashSet<ReviewForUserViewModel>();
            this.ReviewsByUser = new HashSet<ReviewByUserViewModel>();
        }

        public IEnumerable<ReviewForUserViewModel> ReviewsForUser { get; set; }

        public IEnumerable<ReviewByUserViewModel> ReviewsByUser { get; set; }
    }
}
