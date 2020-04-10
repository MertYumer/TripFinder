namespace TripFinder.Web.ViewModels.Cars
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class CarEditInputModel : IMapTo<Car>
    {
        public string Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Make { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Model { get; set; }

        public CarType Type { get; set; }

        public Color Color { get; set; }

        public int? Year { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile NewImage { get; set; }

        [Required]
        public int PassengerSeats { get; set; }

        public bool AllowedSmoking { get; set; }

        public bool AllowedFood { get; set; }

        public bool AllowedDrinks { get; set; }

        public bool PlaceForLuggage { get; set; }

        public bool AllowedPets { get; set; }

        public bool HasAirConditioning { get; set; }
    }
}
