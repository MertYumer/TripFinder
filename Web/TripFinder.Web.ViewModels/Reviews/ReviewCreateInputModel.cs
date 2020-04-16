namespace TripFinder.Web.ViewModels.Reviews
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ReviewCreateInputModel
    {
        [Required]
        public string ReviewerId { get; set; }

        [Required]
        public string ReviewedUserId { get; set; }

        [Required]
        [Range(0, 5)]
        public int Rating { get; set; }

        [MaxLength(50)]
        public string Comment { get; set; }
    }
}
