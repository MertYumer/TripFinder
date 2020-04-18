namespace TripFinder.Web.ViewModels.Reviews
{
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class ReviewByUserViewModel : IMapFrom<Review>
    {
        public string ReviewedUserId { get; set; }

        public string ReviewedUserFirstName { get; set; }

        public string ReviewedUserLastName { get; set; }

        public string ReviewedUserAvatarImageUrl { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
