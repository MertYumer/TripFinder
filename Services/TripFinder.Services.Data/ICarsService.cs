namespace TripFinder.Services.Data
{
    using System.Threading.Tasks;

    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Cars;

    public interface ICarsService
    {
        Task<string> CreateAsync(CarCreateInputModel inputModel, ApplicationUser user);

        Task<string> UpdateAsync(CarEditInputModel inputModel);

        Task<string> DeleteAsync(string id);

        T GetById<T>(string id);

        Car GetById(string id);

        Task<int> GetAllCarsCountAsync();

        Task<int> GetCurrentCarsCountAsync();

        Task<int> GetDeletedCarsCountAsync();
    }
}
