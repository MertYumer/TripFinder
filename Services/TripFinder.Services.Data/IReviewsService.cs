namespace TripFinder.Services.Data
{
    using System.Collections.Generic;

    public interface IReviewsService
    {
        IEnumerable<T> GetLastTripPassengers<T>(string userId);
    }
}
