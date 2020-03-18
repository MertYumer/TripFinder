namespace TripFinder.Web.ViewModels.Trips
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateTripInputModel
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
