namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Users;

    public interface IUsersService
    {
        Task<T> GetById<T>(string id);

        ApplicationUser GetById(string id);

        ApplicationUser GetByIdWithReviews(string id);

        int GetUserNotificationsCount(string id);

        Task<string> CheckForUserByIdAsync(string id);

        Task<string> UpdateAsync(UserEditInputModel inputModel);

        Task<int> UpdateTripUsersAsync(string driverId, IEnumerable<string> passengersIds, int distance);

        Task<string> DeleteAsync(string id);

        Task<int> GetAllUsersCountAsync();

        Task<int> GetActiveUsersCountAsync();

        Task<int> GetDeletedUsersCountAsync();

        Task<IEnumerable<T>> GetAllUsersAsync<T>();

        Task<T> GetDeletedUserDetailsAsync<T>(string id);
    }
}
