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

        int GetUserNotificationsCount(string id);

        string CheckForUserById(string id);

        Task<string> UpdateAsync(UserEditInputModel inputModel);

        Task<int> UpdateTripUsersAsync(string driverId, IEnumerable<string> passengersIds, int distance);

        Task<string> DeleteAsync(string id);
    }
}
