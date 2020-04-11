namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<Notification> notificationsRepository;

        public NotificationsService(IDeletableEntityRepository<Notification> notificationsRepository)
        {
            this.notificationsRepository = notificationsRepository;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var notification = this.notificationsRepository
                .All()
                .FirstOrDefault(t => t.Id == id);

            if (notification == null)
            {
                return null;
            }

            var notificationId = notification.Id;

            this.notificationsRepository.Delete(notification);
            await this.notificationsRepository.SaveChangesAsync();

            return notificationId;
        }

        public Notification GetById(string id)
        {
            var notification = this.notificationsRepository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            return notification;
        }

        public IEnumerable<T> GetUserNotifications<T>(string userId)
        {
            var notifications = this.notificationsRepository
                .All()
                .Where(x => x.ReceiverId == userId || x.SenderId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .To<T>();

            return notifications;
        }
    }
}
