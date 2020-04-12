namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TripFinder.Data.Models;

    public interface INotificationsService
    {
        Task<IEnumerable<T>> GetUserNotifications<T>(string userId);

        Notification GetById(string id);

        Task<string> DeleteAsync(string id);
    }
}
