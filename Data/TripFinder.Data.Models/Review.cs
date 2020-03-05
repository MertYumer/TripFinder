namespace TripFinder.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using TripFinder.Data.Common.Models;

    public class Review : BaseModel<string>
    {
        public Review()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
        }

        [Required]
        public string ReviewerId { get; set; }

        public virtual User Reviewer { get; set; }

        [Required]
        public string ReviewedUserId { get; set; }

        public virtual User ReviewedUser { get; set; }

        [Required]
        [Range(0, 5)]
        public double Rating { get; set; }

        [MinLength(5)]
        [MaxLength(50)]
        public string Comment { get; set; }
    }
}
