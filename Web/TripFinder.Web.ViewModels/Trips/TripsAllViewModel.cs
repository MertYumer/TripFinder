namespace TripFinder.Web.ViewModels.Trips
{
    using System.Collections.Generic;

    public class TripsAllViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public IEnumerable<TripViewModel> AllTrips { get; set; }
    }
}
