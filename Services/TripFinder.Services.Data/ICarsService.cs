namespace TripFinder.Services.Data
{
    using System.Threading.Tasks;

    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Cars;

    public interface ICarsService
    {
        Task<string> CreateAsync(CarCreateInputModel inputModel, ApplicationUser user);

        Task<string> UpdateAsync(CarEditViewModel inputModel);

        T GetById<T>(string id);
    }
}
