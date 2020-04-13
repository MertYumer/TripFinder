namespace TripFinder.Web.ViewModels.Trips
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class TripEditViewModel : IMapFrom<Trip>
    {
        public string Id { get; set; }

        public ApplicationUser Driver { get; set; }

        [Required]
        public Town Origin { get; set; }

        [Required]
        public Town Destination { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfDeparture { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime TimeOfDeparture { get; set; }

        [Required]
        public int FreeSeats { get; set; }

        [Required]
        public decimal ExpensePerPerson { get; set; }

        [MaxLength(100)]
        public string AdditionalInformation { get; set; }
    }
}
