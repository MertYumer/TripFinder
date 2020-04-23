namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IReviewsService
    {
        Task<IEnumerable<T>> GetPendingReviewsAsync<T>(string userId);

        Task<IEnumerable<T>> GetReviewsForUserAsync<T>(string userId);

        Task<IEnumerable<T>> GetReviewsByUserAsync<T>(string userId);

        Task<bool> AddReviewsAsync(IFormCollection data, string userId);
    }
}
