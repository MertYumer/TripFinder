namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IReviewsService
    {
        Task<IEnumerable<T>> GetLastTripPassengers<T>(string userId);

        Task<bool> AddReviews(IFormCollection data, string userId);
    }
}
