namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TripFinder.Data.Models;

    public interface INotificationsService
    {
        Task<IEnumerable<T>> GetUserNotificationsAsync<T>(string userId);

        Task<Notification> GetByIdAsync(string id);

        Task<string> DeleteAsync(string id);

        Task<string> SendNotificationAsync(string receiverId, string senderId, string tripId, NotificationSubject subject);
    }
}
