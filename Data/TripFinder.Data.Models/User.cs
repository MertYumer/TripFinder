namespace TripFinder.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using TripFinder.Data.Common.Models;

    public class User : BaseDeletableModel<string>
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ReviewsForUser = new HashSet<Review>();
            this.ReviewsByUser = new HashSet<Review>();
            this.UserTrips = new HashSet<UserTrip>();
            this.CreatedOn = DateTime.UtcNow;
        }

        [Required]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        public string Password { get; set; }

        public string AvatarImage { get; set; }

        public int? Age { get; set; }

        public Gender? Gender { get; set; }

        [Required]
        [RegularExpression(@"^(087|088|089|098)[0-9]{7}$")]
        public string PhoneNumber { get; set; }

        public int TripsCountAsDriver { get; set; }

        public int TripsCountAsPassenger { get; set; }

        public int RatingsCount { get; set; }

        [Required]
        [Range(0.0, 5)]
        public double Rating { get; set; }

        public long TravelledDistance { get; set; }

        public virtual Car Car { get; set; }

        public virtual ICollection<Review> ReviewsForUser { get; set; }

        public virtual ICollection<Review> ReviewsByUser { get; set; }

        public virtual ICollection<UserTrip> UserTrips { get; set; }
    }
}
