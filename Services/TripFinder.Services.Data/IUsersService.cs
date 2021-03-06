﻿namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Users;

    public interface IUsersService
    {
        Task<T> GetByIdAsync<T>(string id);

        Task<ApplicationUser> GetByIdAsync(string id);

        Task<ApplicationUser> GetByIdWithReviewsAsync(string id);

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
