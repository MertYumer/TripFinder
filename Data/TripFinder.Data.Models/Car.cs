namespace TripFinder.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TripFinder.Data.Common.Models;

    public class Car : BaseDeletableModel<string>
    {
        public Car()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Trips = new HashSet<Trip>();
            this.CreatedOn = DateTime.UtcNow;
        }

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

        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        [Required]
        public int PassengerSeats { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

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

        public virtual ICollection<Trip> Trips { get; set; }
    }
}
