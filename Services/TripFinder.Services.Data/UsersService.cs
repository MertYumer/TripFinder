namespace TripFinder.Services.Data
{
    using System.Linq;

    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public UserDetailsViewModel GetUserById(string userId)
        {
            var user = this.usersRepository
                .AllAsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new UserDetailsViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    AvatarImage = u.AvatarImage,
                    Gender = u.Gender.ToString(),
                    PhoneNumber = u.PhoneNumber,
                    TripsCountAsDriver = u.TripsCountAsDriver,
                    TripsCountAsPassenger = u.TripsCountAsPassenger,
                    RatingsCount = u.RatingsCount,
                    Rating = u.Rating,
                    TravelledDistance = u.TravelledDistance,
                })
                .SingleOrDefault();

            return user;
        }
    }
}
