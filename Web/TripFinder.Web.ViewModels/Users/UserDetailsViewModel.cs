namespace TripFinder.Web.ViewModels.Users
{
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class UserDetailsViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string AvatarImage { get; set; }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; }

        public int TripsCountAsDriver { get; set; }

        public int TripsCountAsPassenger { get; set; }

        public int RatingsCount { get; set; }

        public double Rating { get; set; }

        public long TravelledDistance { get; set; }
    }
}
