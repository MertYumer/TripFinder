namespace TripFinder.Services.Data
{
    using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<T>> GetUserNotifications<T>(string userId)
        {
            var notifications = new List<T>();

            var receivedNotifications = await this.notificationsRepository
                .All()
                .Where(x => x.ReceiverId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .Take(5)
                .To<T>()
                .ToListAsync();

            var sentNotifications = await this.notificationsRepository
                .All()
                .Where(x => x.SenderId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .Take(5)
                .To<T>()
                .ToListAsync();

            notifications.AddRange(receivedNotifications);
            notifications.AddRange(sentNotifications);

            return notifications;
        }
    }
}
