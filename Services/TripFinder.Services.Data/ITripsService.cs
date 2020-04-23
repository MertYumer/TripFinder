namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Trips;

    public interface ITripsService
    {
        Task<string> CreateAsync(TripCreateInputModel inputModel, ApplicationUser user);

        Task UpdateTripViewsCountAsync(string id);

        Task<string> UpdateAsync(TripEditInputModel inputModel);

        Task<string> DeleteAsync(string id);

        Task<T> GetByIdAsync<T>(string id);

        Task<Trip> GetByIdAsync(string id);

        Task<IEnumerable<T>> GetAllTripsWithDeletedAsync<T>();

        Task<IEnumerable<T>> GetAllTripsAsync<T>(int take, int skip = 0);

        Task<IEnumerable<T>> GetMyTripsAsync<T>(string userId, int take, int skip = 0);

        Task<IEnumerable<T>> ShowSearchResultsAsync<T>(TripSearchInputModel inputModel, string userId, int take, int skip = 0);

        Task<int> GetMyTripsCountAsync(string userId);

        Task<int> GetSearchResultsCountAsync(TripSearchInputModel inputModel, string userId);

        Task<string> AddUserToTripAsync(string requestorId, string tripCreatorId, string tripId);

        Task<string> CompleteAsync(string tripId, string userId);

        IEnumerable<string> GetDriverAndPassengersIds(string id);

        Task<bool> CheckForUserTripAsync(string userId, string tripId);

        Task<int> GetAllTripsCountAsync();

        Task<int> GetAllTripsCountWithDeletedAsync();

        Task<int> GetActiveTripsCountAsync();

        Task<int> GetDeletedTripsCountAsync();

        Task<T> GetDeletedTripDetailsAsync<T>(string id);
    }
}
