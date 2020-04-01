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

        T GetById<T>(string id);

        IEnumerable<T> GetAllTrips<T>();
    }
}
