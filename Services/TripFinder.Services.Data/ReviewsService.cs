﻿namespace TripFinder.Services.Data
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

        private readonly IUsersService usersService;

        public ReviewsService(
            IDeletableEntityRepository<Trip> tripsRepository,
            IRepository<ApplicationUser> usersRepository,
            IRepository<UserTrip> userTripsRepository,
            IUsersService usersService)
        {
            this.tripsRepository = tripsRepository;
            this.usersRepository = usersRepository;
            this.userTripsRepository = userTripsRepository;
            this.usersService = usersService;
        }

        public async Task<IEnumerable<T>> GetLastTripPassengers<T>(string userId)
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

            var passengers = await this.userTripsRepository
                .All()
                .Where(ut => ut.UserId != userId && ut.TripId == lastTrip.Id)
                .Select(ut => ut.User)
                .To<T>()
                .ToListAsync();

            return passengers;
        }

        public async Task<bool> AddReviews(IFormCollection data, string userId)
        {
            var user = this.usersService.GetByIdWithReviews(userId);
            var reviewsCount = data.Count / 3;

            for (int i = 0; i < reviewsCount; i++)
            {
                var reviewedUserId = data[$"id-{i}"];
                var reviewedUser = this.usersService.GetByIdWithReviews(reviewedUserId);

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

            var userGaveRatings = this.UpdateUserTrip(user.Id).Result;

            return userGaveRatings;
        }

        private async Task<bool> UpdateUserTrip(string userId)
        {
            var lastTripId = this.tripsRepository
                .AllWithDeleted()
                .Include(t => t.UserTrips)
                .Where(t => t.IsDeleted && t.UserTrips.Any(ut => ut.UserId == userId))
                .OrderByDescending(t => t.DeletedOn)
                .FirstOrDefaultAsync()
                .Result
                .Id;

            var userTrip = this.userTripsRepository
                .All()
                .FirstOrDefaultAsync(ut => ut.TripId == lastTripId && ut.UserId == userId)
                .Result;

            userTrip.GaveRatings = true;
            this.userTripsRepository.Update(userTrip);
            await this.userTripsRepository.SaveChangesAsync();

            return userTrip.GaveRatings;
        }
    }
}