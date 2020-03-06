// ReSharper disable VirtualMemberCallInConstructor
namespace TripFinder.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;
    using TripFinder.Data.Common.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.ReviewsForUser = new HashSet<Review>();
            this.ReviewsByUser = new HashSet<Review>();
            this.UserTrips = new HashSet<UserTrip>();
            this.CreatedOn = DateTime.UtcNow;
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [MinLength(3)]
        [MaxLength(20)]
        public override string Email { get; set; }

        public string AvatarImage { get; set; }

        public int? Age { get; set; }

        public Gender? Gender { get; set; }

        [RegularExpression(@"^(087|088|089|098)[0-9]{7}$")]
        public override string PhoneNumber { get; set; }

        public int TripsCountAsDriver { get; set; }

        public int TripsCountAsPassenger { get; set; }

        public int RatingsCount { get; set; }

        [Required]
        [Range(0.0, 5)]
        public double Rating { get; set; }

        public long TravelledDistance { get; set; }

        public string CarId { get; set; }

        public virtual Car Car { get; set; }

        public virtual ICollection<Review> ReviewsForUser { get; set; }

        public virtual ICollection<Review> ReviewsByUser { get; set; }

        public virtual ICollection<UserTrip> UserTrips { get; set; }
    }
}
