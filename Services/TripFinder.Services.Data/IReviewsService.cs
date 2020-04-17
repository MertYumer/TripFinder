namespace TripFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using TripFinder.Data.Models;

    public interface IReviewsService
    {
        IEnumerable<T> GetLastTripPassengers<T>(string userId);

        Task<bool> AddReviews(IFormCollection data, string userId);
    }
}
