namespace TripFinder.Web.ViewModels.Cars
{
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class CarDeleteViewModel : IMapFrom<Car>
    {
        public string Id { get; set; }
    }
}
