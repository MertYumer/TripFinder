namespace TripFinder.Web.ViewModels.Trips
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TripSearchInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Origin { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Destination { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfDeparture { get; set; }

        [Required]
        public int SeatsNeeded { get; set; }
    }
}
