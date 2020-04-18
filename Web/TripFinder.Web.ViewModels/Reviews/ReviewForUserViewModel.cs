namespace TripFinder.Web.ViewModels.Reviews
{
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class ReviewForUserViewModel : IMapFrom<Review>
    {
        public string ReviewerId { get; set; }

        public string ReviewerFirstName { get; set; }

        public string ReviewerLastName { get; set; }

        public string ReviewerAvatarImageUrl { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
