namespace TripFinder.Web.ViewModels.Cars
{
    using System.ComponentModel.DataAnnotations;

    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class CarCreateInputModel : IMapTo<Car>
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Make { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Model { get; set; }

        public CarType Type { get; set; }

        public string Color { get; set; }

        public int? Year { get; set; }

        public string Image { get; set; }

        [Required]
        public int PassengerSeats { get; set; }

        [Required]
        public bool AllowedSmoking { get; set; }

        [Required]
        public bool AllowedFood { get; set; }

        [Required]
        public bool AllowedDrinks { get; set; }

        [Required]
        public bool PlaceForLuggage { get; set; }

        [Required]
        public bool AllowedPets { get; set; }

        [Required]
        public bool HasAirConditioning { get; set; }
    }
}
