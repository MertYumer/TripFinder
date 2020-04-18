namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IReviewsService
    {
        Task<IEnumerable<T>> GetPendingReviews<T>(string userId);

        Task<IEnumerable<T>> GetReviewsForUser<T>(string userId);

        Task<IEnumerable<T>> GetReviewsByUser<T>(string userId);

        Task<bool> AddReviews(IFormCollection data, string userId);
    }
}
