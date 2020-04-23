namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class ReviewsService : IReviewsService
    {
        private readonly IDeletableEntityRepository<Trip> tripsRepository;
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IRepository<UserTrip> userTripsRepository;
        private readonly IRepository<Review> reviewsRepository;

        private readonly IUsersService usersService;

        public ReviewsService(
            IDeletableEntityRepository<Trip> tripsRepository,
            IRepository<ApplicationUser> usersRepository,
            IRepository<UserTrip> userTripsRepository,
            IRepository<Review> reviewsRepository,
            IUsersService usersService)
        {
            this.tripsRepository = tripsRepository;
            this.usersRepository = usersRepository;
            this.userTripsRepository = userTripsRepository;
            this.reviewsRepository = reviewsRepository;
            this.usersService = usersService;
        }

        public async Task<IEnumerable<T>> GetPendingReviewsAsync<T>(string userId)
        {
            var lastTrip = await this.tripsRepository
                .AllWithDeleted()
                .Include(t => t.UserTrips)
                .ThenInclude(ut => ut.User)
                .ThenInclude(u => u.AvatarImage)
                .Where(t => t.IsDeleted && t.UserTrips.Any(ut => ut.UserId == userId && !ut.GaveRatings))
                .OrderByDescending(t => t.DeletedOn)
                .FirstOrDefaultAsync();

            if (lastTrip == null)
            {
                return null;
            }

            var pendingReviews = await this.userTripsRepository
                .All()
                .Where(ut => ut.UserId != userId && ut.TripId == lastTrip.Id)
                .Select(ut => ut.User)
                .To<T>()
                .ToListAsync();

            return pendingReviews;
        }

        public async Task<bool> AddReviewsAsync(IFormCollection data, string userId)
        {
            var user = await this.usersService.GetByIdWithReviewsAsync(userId);
            var reviewsCount = data.Count / 3;

            for (int i = 0; i < reviewsCount; i++)
            {
                var reviewedUserId = data[$"id-{i}"];
                var reviewedUser = await this.usersService.GetByIdWithReviewsAsync(reviewedUserId);

                var review = new Review
                {
                    ReviewerId = user.Id,
                    ReviewedUserId = reviewedUser.Id,
                    Rating = int.Parse(data[$"rating-{i}"]),
                    Comment = data[$"comment-{i}"],
                };

                user.ReviewsByUser.Add(review);
                reviewedUser.ReviewsForUser.Add(review);

                reviewedUser.RatingsCount = reviewedUser.ReviewsForUser.Count;
                reviewedUser.Rating = (double)reviewedUser.ReviewsForUser.Sum(r => r.Rating) / (double)reviewedUser.RatingsCount;

                this.usersRepository.Update(reviewedUser);
            }

            if (user.ReviewsForUser.Count > 0)
            {
                user.RatingsCount = user.ReviewsForUser.Count;
                user.Rating = (double)user.ReviewsForUser.Sum(r => r.Rating) / (double)user.RatingsCount;
            }

            user.HasUsersToReview = false;

            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();

            var userGaveRatings = this.UpdateUserTripAsync(user.Id).Result;

            return userGaveRatings;
        }

        public async Task<IEnumerable<T>> GetReviewsForUserAsync<T>(string userId)
        {
            var reviewsForUser = await this.reviewsRepository
                .All()
                .Include(r => r.ReviewedUser)
                .ThenInclude(ru => ru.AvatarImage)
                .Where(r => r.ReviewedUserId == userId)
                .OrderByDescending(r => r.CreatedOn)
                .Take(5)
                .To<T>()
                .ToListAsync();

            return reviewsForUser;
        }

        public async Task<IEnumerable<T>> GetReviewsByUserAsync<T>(string userId)
        {
            var reviewsByUser = await this.reviewsRepository
                .All()
                .Include(r => r.Reviewer)
                .ThenInclude(ru => ru.AvatarImage)
                .Where(r => r.ReviewerId == userId)
                .OrderByDescending(r => r.CreatedOn)
                .Take(5)
                .To<T>()
                .ToListAsync();

            return reviewsByUser;
        }

        private async Task<bool> UpdateUserTripAsync(string userId)
        {
            var lastTripId = this.tripsRepository
                .AllWithDeleted()
                .Include(t => t.UserTrips)
                .Where(t => t.IsDeleted && t.UserTrips.Any(ut => ut.UserId == userId))
                .OrderByDescending(t => t.DeletedOn)
                .FirstOrDefaultAsync()
                .Result
                .Id;

            var userTrip = await this.userTripsRepository
                .All()
                .FirstOrDefaultAsync(ut => ut.TripId == lastTripId && ut.UserId == userId);

            userTrip.GaveRatings = true;
            this.userTripsRepository.Update(userTrip);
            await this.userTripsRepository.SaveChangesAsync();

            return userTrip.GaveRatings;
        }
    }
}
