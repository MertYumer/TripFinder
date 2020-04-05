namespace TripFinder.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class UserEditViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

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
        public string Email { get; set; }

        public string AvatarImageId { get; set; }

        public string AvatarImageUrl { get; set; }

        public IFormFile NewImage { get; set; }

        public int? Age { get; set; }

        public Gender? Gender { get; set; }

        [RegularExpression(@"^(087|088|089|098)[0-9]{7}$")]
        public string PhoneNumber { get; set; }
    }
}
