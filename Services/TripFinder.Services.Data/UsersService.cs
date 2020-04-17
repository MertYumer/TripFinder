namespace TripFinder.Services.Data
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

        public string CheckForUserById(string id)
        {
            var user = this.usersRepository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            return user.Id;
        }

        public int GetUserNotificationsCount(string id)
        {
            var notificationsCount = this.usersRepository
                .All()
                .Include(u => u.ReceivedNotifications)
                .Where(x => x.Id == id)
                .FirstOrDefault()
                .ReceivedNotifications
                .Count;

            return notificationsCount;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var user = this.usersRepository
                .All()
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            var userId = user.Id;

            this.usersRepository.Delete(user);
            await this.usersRepository.SaveChangesAsync();

            return userId;
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

        public ApplicationUser GetById(string id)
        {
            var user = this.usersRepository
                .All()
                .Include(u => u.UserTrips)
                .FirstOrDefault(x => x.Id == id);

            return user;
        }

        public ApplicationUser GetByIdWithReviews(string id)
        {
            var user = this.usersRepository
                .All()
                .Include(u => u.ReviewsByUser)
                .Include(u => u.ReviewsForUser)
                .FirstOrDefault(x => x.Id == id);

            return user;
        }

        public async Task<string> UpdateAsync(UserEditInputModel inputModel)
        {
            var user = this.usersRepository
                .All()
                .FirstOrDefault(u => u.Id == inputModel.Id);

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

            var driver = this.GetById(driverId);
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
                var passenger = this.GetById(passengerId);
                passenger.TripsCountAsPassenger++;
                passenger.TravelledDistance += distance;
                passenger.HasUsersToReview = true;
                this.usersRepository.Update(passenger);
                updatedUsersCount++;
            }

            await this.usersRepository.SaveChangesAsync();

            return updatedUsersCount;
        }
    }
}
