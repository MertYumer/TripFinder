namespace TripFinder.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TripFinder.Data.Common.Models;

    public class Trip : BaseDeletableModel<string>
    {
        public Trip()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserTrips = new HashSet<UserTrip>();
            this.CreatedOn = DateTime.UtcNow;
        }

        public string DriverId { get; set; }

        public virtual ApplicationUser Driver { get; set; }

        [Required]
        public string TownsDistanceId { get; set; }

        public virtual TownsDistance TownsDistance { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public DateTime DateOfDeparture { get; set; }

        [Required]
        public DateTime TimeOfDeparture { get; set; }

        [Required]
        public string CarId { get; set; }

        public virtual Car Car { get; set; }

        [Required]
        public int TotalSeats { get; set; }

        [Required]
        public int FreeSeats { get; set; }

        [Required]
        public decimal ExpensePerPerson { get; set; }

        public string AdditionalInformation { get; set; }

        public int Views { get; set; }

        public virtual ICollection<UserTrip> UserTrips { get; set; }
    }
}
