namespace TripFinder.Web.ViewModels.Administration.Dashboard
{
    using System;

    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime DeletedOn { get; set; }
    }
}
