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
        private readonly IRepository<Notification> notificationsRepository;
        private readonly IImagesService imagesService;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IRepository<Notification> notificationsRepository,
            IImagesService imagesService)
        {
            this.usersRepository = usersRepository;
            this.notificationsRepository = notificationsRepository;
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
                .Where(x => x.Id == id)
                .FirstOrDefault();

            return user;
        }

        public async Task<string> SendNotificationAsync(ApplicationUser receiver, ApplicationUser sender, string tripId, string subject)
        {
            var notification = new Notification
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
                TripId = tripId,
                Subject = subject,
            };

            receiver.ReceivedNotifications.Add(notification);
            this.usersRepository.Update(receiver);

            sender.SentNotifications.Add(notification);
            this.usersRepository.Update(sender);

            await this.usersRepository.SaveChangesAsync();

            return notification.Id;
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

        public IEnumerable<T> GetUserNotifications<T>(string userId)
        {
            var notifications = this.notificationsRepository
                .All()
                .Where(x => x.ReceiverId == userId)
                .To<T>();

            return notifications;
        }
    }
}
