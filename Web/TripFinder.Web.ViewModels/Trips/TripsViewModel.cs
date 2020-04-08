namespace TripFinder.Web.ViewModels.Trips
{
    using System.Collections.Generic;

    public class TripsViewModel
    {
        public string Title { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public IEnumerable<TripViewModel> Trips { get; set; }
    }
}
