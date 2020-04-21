namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Cars;

    public interface ICarsService
    {
        Task<string> CreateAsync(CarCreateInputModel inputModel, ApplicationUser user);

        Task<string> UpdateAsync(CarEditInputModel inputModel);

        Task<string> DeleteAsync(string id);

        Task<T> GetByIdAsync<T>(string id);

        Task<Car> GetByIdAsync(string id);

        Task<int> GetAllCarsCountAsync();

        Task<int> GetCurrentCarsCountAsync();

        Task<int> GetDeletedCarsCountAsync();

        Task<IEnumerable<T>> GetAllCarsAsync<T>();

        Task<T> GetDeletedCarDetailsAsync<T>(string id);
    }
}
