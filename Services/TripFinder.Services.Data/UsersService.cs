namespace TripFinder.Services.Data
{
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

        public async Task<string> SendNotificationAsync(string receiverId, string senderId, Trip trip, NotificationSubject subject)
        {
            var receiver = this.GetById(receiverId);
            var sender = this.GetById(senderId);

            if (receiver == null || sender == null || trip == null)
            {
                return null;
            }

            if (receiver.UserTrips.All(ut => ut.TripId != trip.Id) || sender.UserTrips.Any(ut => ut.TripId == trip.Id))
            {
                return null;
            }

            if (trip.FreeSeats == 0)
            {
                return null;
            }

            var notification = new Notification
            {
                SenderId = sender.Id,
                ReceiverId = receiver.Id,
                TripId = trip.Id,
                Subject = subject,
            };

            if (notification == null)
            {
                return null;
            }

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
    }
}
