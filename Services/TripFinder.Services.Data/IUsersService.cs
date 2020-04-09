namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Users;

    public interface IUsersService
    {
        T GetById<T>(string id);

        ApplicationUser GetById(string id);

        string CheckForUserById(string id);

        Task<string> UpdateAsync(UserEditInputModel inputModel);

        Task<string> DeleteAsync(string id);

        Task<string> SendNotificationAsync(ApplicationUser receiver, ApplicationUser sender, string tripId, string subject);

        IEnumerable<T> GetUserNotifications<T>(string userId);
    }
}
