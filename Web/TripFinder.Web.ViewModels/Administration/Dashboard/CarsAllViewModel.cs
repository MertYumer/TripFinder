namespace TripFinder.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;

    public class CarsAllViewModel
    {
        public CarsAllViewModel()
        {
            this.Cars = new HashSet<CarViewModel>();
        }

        public IEnumerable<CarViewModel> Cars { get; set; }
    }
}
