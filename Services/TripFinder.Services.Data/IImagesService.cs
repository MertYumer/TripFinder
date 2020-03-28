namespace TripFinder.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using TripFinder.Data.Models;

    public interface IImagesService
    {
        Task<Image> CreateAsync(IFormFile imageSource);

        Task DeleteAsync(string id);
    }
}
