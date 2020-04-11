namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

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
