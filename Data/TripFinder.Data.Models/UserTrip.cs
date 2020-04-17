namespace TripFinder.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserTrip
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string TripId { get; set; }

        public virtual Trip Trip { get; set; }

        public bool GaveRatings { get; set; }
    }
}
