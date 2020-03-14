namespace TripFinder.Services.Data
{
    using System.Linq;

    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;
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
                .To<UserDetailsViewModel>()
                .SingleOrDefault();

            return user;
        }
    }
}
