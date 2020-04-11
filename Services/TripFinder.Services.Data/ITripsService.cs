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

        T GetById<T>(string id);

        Trip GetById(string id);

        Task<IEnumerable<T>> GetAllTrips<T>(int? take = null, int skip = 0);

        Task<IEnumerable<T>> GetMyTrips<T>(string userId, int? take = null, int skip = 0);

        Task<IEnumerable<T>> ShowSearchResults<T>(TripSearchInputModel inputModel, string userId, int? take = null, int skip = 0);

        Task DeletePassedTripsAsync();

        int GetAllTripsCount();

        int GetMyTripsCount(string userId);

        int GetSearchResultsCount(TripSearchInputModel inputModel, string userId);

        Task<string> AddUserToTripAsync(string requestorId, string tripCreatorId, Trip trip);
    }
}
