namespace TripFinder.Services.Data
{
    using System.Linq;

    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public T GetById<T>(string id)
        {
            var user = this.usersRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return user;
        }
    }
}
