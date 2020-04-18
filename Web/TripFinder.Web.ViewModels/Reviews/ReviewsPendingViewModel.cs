namespace TripFinder.Web.ViewModels.Reviews
{
    using System.Collections.Generic;

    public class ReviewsPendingViewModel
    {
        public ReviewsPendingViewModel()
        {
            this.Reviews = new HashSet<ReviewPendingViewModel>();
        }

        public IEnumerable<ReviewPendingViewModel> Reviews { get; set; }
    }
}
