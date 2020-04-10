namespace TripFinder.Services.Data
{
    using System.Collections.Generic;

    using TripFinder.Data.Models;

    public interface INotificationsService
    {
        IEnumerable<T> GetUserNotifications<T>(string userId);

        Notification GetById(string id);
    }
}
