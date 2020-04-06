namespace TripFinder.Web.ViewModels.Trips
{
    using System.Collections.Generic;

    public class TripsMyViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public IEnumerable<TripViewModel> MyTrips { get; set; }
    }
}
