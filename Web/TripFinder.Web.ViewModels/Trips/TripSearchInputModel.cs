namespace TripFinder.Web.ViewModels.Trips
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using TripFinder.Data.Models;

    public class TripSearchInputModel
    {
        [Required]
        public string Origin { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfDeparture { get; set; }

        [Required]
        public int SeatsNeeded { get; set; }
    }
}
