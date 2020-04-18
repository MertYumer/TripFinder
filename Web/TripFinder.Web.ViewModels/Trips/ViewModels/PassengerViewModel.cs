namespace TripFinder.Web.ViewModels.Trips
{
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class PassengerViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string AvatarImageUrl { get; set; }
    }
}
