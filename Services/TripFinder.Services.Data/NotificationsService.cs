namespace TripFinder.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<Notification> notificationsRepository;
        private readonly IRepository<ApplicationUser> usersRepository;

        private readonly IUsersService usersService;
        private readonly ITripsService tripsService;

        public NotificationsService(
            IDeletableEntityRepository<Notification> notificationsRepository,
            IRepository<ApplicationUser> usersRepository,
            IUsersService usersService,
            ITripsService tripsService)
        {
            this.notificationsRepository = notificationsRepository;
            this.usersRepository = usersRepository;
            this.usersService = usersService;
            this.tripsService = tripsService;
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
            await this.DeletePassedNotificationsAsync(userId);

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

        public async Task<string> SendNotificationAsync(string receiverId, string senderId, string tripId, NotificationSubject subject)
        {
            var receiver = this.usersService.GetById(receiverId);
            var sender = this.usersService.GetById(senderId);
            var trip = this.tripsService.GetById(tripId);

            if (receiver == null || sender == null || trip == null)
            {
                return null;
            }

            var userTripExists = this.tripsService.CheckForUserTrip(receiver.Id, tripId);

            if ((subject == NotificationSubject.RequestJoin || subject == NotificationSubject.AcceptRequest)
                && trip.FreeSeats == 0
                && !userTripExists)
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

            receiver.ReceivedNotifications.Add(notification);
            this.usersRepository.Update(receiver);

            sender.SentNotifications.Add(notification);
            this.usersRepository.Update(sender);

            await this.usersRepository.SaveChangesAsync();

            return notification.Id;
        }

        private async Task DeletePassedNotificationsAsync(string userId)
        {
            var passedNotifications = await this.notificationsRepository
                .AllWithDeleted()
                .Where(n => n.IsDeleted && (n.ReceiverId == userId || n.SenderId == userId))
                .Where(n => n.DeletedOn <= DateTime.UtcNow.AddDays(3))
                .ToListAsync();

            foreach (var notification in passedNotifications)
            {
                this.notificationsRepository.HardDelete(notification);
            }

            await this.notificationsRepository.SaveChangesAsync();
        }
    }
}
