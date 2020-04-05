namespace TripFinder.Web.ViewModels.Trips
{
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class TripDeleteViewModel : IMapFrom<Trip>
    {
        public string Id { get; set; }

        public string DriverId { get; set; }
    }
}
