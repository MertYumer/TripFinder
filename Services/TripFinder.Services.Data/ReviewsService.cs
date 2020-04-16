namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class ReviewsService : IReviewsService
    {
        private readonly IDeletableEntityRepository<Trip> tripsRepository;

        private readonly IUsersService usersService;

        public ReviewsService(
            IDeletableEntityRepository<Trip> tripsRepository,
            IUsersService usersService)
        {
            this.tripsRepository = tripsRepository;
            this.usersService = usersService;
        }

        public IEnumerable<T> GetLastTripPassengers<T>(string userId)
        {
            var lastTrip = this.tripsRepository
                .AllWithDeleted()
                .Include(t => t.UserTrips)
                .ThenInclude(ut => ut.User)
                .Where(t => t.IsDeleted && t.UserTrips.Any(ut => ut.UserId == userId))
                .OrderByDescending(t => t.DeletedOn)
                .FirstOrDefault();

            if (lastTrip == null)
            {
                return null;
            }

            var passengers = lastTrip
                .UserTrips
                .Where(ut => ut.UserId != userId)
                .Select(ut => ut.User)
                .AsQueryable()
                .To<T>()
                .ToList();

            return passengers;
        }
    }
}
