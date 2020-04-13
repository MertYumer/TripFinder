namespace TripFinder.Web.ViewModels.Trips
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using TripFinder.Data.Models;

    public class TripSearchInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public Town Origin { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public Town Destination { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfDeparture { get; set; }

        [Required]
        public int SeatsNeeded { get; set; }
    }
}
