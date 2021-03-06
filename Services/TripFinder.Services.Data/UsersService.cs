﻿namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;
    using TripFinder.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        private readonly IImagesService imagesService;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IImagesService imagesService)
        {
            this.usersRepository = usersRepository;
            this.imagesService = imagesService;
        }

        public async Task<string> CheckForUserByIdAsync(string id)
        {
            var user = await this.usersRepository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            return user.Id;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var user = await this.usersRepository
                .All()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            var userId = user.Id;

            this.usersRepository.Delete(user);
            await this.usersRepository.SaveChangesAsync();

            return userId;
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            var user = await this.usersRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            var user = await this.usersRepository
                .All()
                .Include(u => u.UserTrips)
                .FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<ApplicationUser> GetByIdWithReviewsAsync(string id)
        {
            var user = await this.usersRepository
                .All()
                .Include(u => u.ReviewsByUser)
                .Include(u => u.ReviewsForUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public async Task<string> UpdateAsync(UserEditInputModel inputModel)
        {
            var user = await this.usersRepository
                .All()
                .FirstOrDefaultAsync(u => u.Id == inputModel.Id);

            if (user == null)
            {
                return null;
            }

            user.FirstName = inputModel.FirstName;
            user.LastName = inputModel.LastName;
            user.Email = inputModel.Email;
            user.Age = inputModel.Age;
            user.Gender = inputModel.Gender;
            user.PhoneNumber = inputModel.PhoneNumber;

            if (inputModel.NewImage != null)
            {
                var oldImageId = user.AvatarImageId;
                var newImage = await this.imagesService.CreateAsync(inputModel.NewImage);
                user.AvatarImageId = newImage.Id;
                await this.imagesService.DeleteAsync(oldImageId);
            }

            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();

            return user.Id;
        }

        public async Task<int> UpdateTripUsersAsync(string driverId, IEnumerable<string> passengersIds, int distance)
        {
            var updatedUsersCount = 0;

            var driver = await this.GetByIdAsync(driverId);
            driver.TripsCountAsDriver++;
            driver.TravelledDistance += distance;

            if (passengersIds.Count() > 0)
            {
                driver.HasUsersToReview = true;
            }

            this.usersRepository.Update(driver);
            updatedUsersCount++;

            foreach (var passengerId in passengersIds)
            {
                var passenger = await this.GetByIdAsync(passengerId);
                passenger.TripsCountAsPassenger++;
                passenger.TravelledDistance += distance;
                passenger.HasUsersToReview = true;
                this.usersRepository.Update(passenger);
                updatedUsersCount++;
            }

            await this.usersRepository.SaveChangesAsync();

            return updatedUsersCount;
        }

        public async Task<int> GetAllUsersCountAsync()
        {
            var allUsersCount = await this.usersRepository
                .AllWithDeleted()
                .Where(u => u.Email != "admin@tripfinder.com")
                .CountAsync();

            return allUsersCount;
        }

        public async Task<int> GetActiveUsersCountAsync()
        {
            var activeUsersCount = await this.usersRepository
                .All()
                .Where(u => u.Email != "admin@tripfinder.com")
                .CountAsync();

            return activeUsersCount;
        }

        public async Task<int> GetDeletedUsersCountAsync()
        {
            var deletedUsersCount = await this.usersRepository
                .AllWithDeleted()
                .Where(u => u.IsDeleted)
                .CountAsync();

            return deletedUsersCount;
        }

        public async Task<IEnumerable<T>> GetAllUsersAsync<T>()
        {
            var allUsers = await this.usersRepository
                .AllWithDeleted()
                .Where(u => u.Email != "admin@tripfinder.com")
                .To<T>()
                .ToListAsync();

            return allUsers;
        }

        public async Task<T> GetDeletedUserDetailsAsync<T>(string id)
        {
            var user = await this.usersRepository
                .AllWithDeleted()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return user;
        }
    }
}
