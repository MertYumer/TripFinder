namespace TripFinder.Web.ViewModels.Cars
{
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class CarDetailsViewModel : IMapFrom<Car>
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public CarType Type { get; set; }

        public string Color { get; set; }

        public int? Year { get; set; }

        public string Image { get; set; }

        public int PassengerSeats { get; set; }

        public bool AllowedSmoking { get; set; }

        public bool AllowedFood { get; set; }

        public bool AllowedDrinks { get; set; }

        public bool PlaceForLuggage { get; set; }

        public bool AllowedPets { get; set; }

        public bool HasAirConditioning { get; set; }
    }
}
