namespace TripFinder.Web.ViewModels.Reviews
{
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class UserReviewViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
