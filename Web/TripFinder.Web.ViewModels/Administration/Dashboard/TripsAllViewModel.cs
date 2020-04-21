namespace TripFinder.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;

    public class TripsAllViewModel
    {
        public TripsAllViewModel()
        {
            this.Trips = new HashSet<TripViewModel>();
        }

        public IEnumerable<TripViewModel> Trips { get; set; }
    }
}
