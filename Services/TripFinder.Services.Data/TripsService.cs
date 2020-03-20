namespace TripFinder.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Trips;

    public class TripsService : ITripsService
    {
        private readonly IRepository<Trip> tripsRepository;
        private readonly IRepository<TownsDistance> townsDistancesRepository;
        private readonly IRepository<UserTrip> userTripsRepository;

        public TripsService(
            IRepository<Trip> tripsRepository, 
            IRepository<TownsDistance> townsDistancesRepository, 
            IRepository<UserTrip> userTripsRepository)
        {
            this.tripsRepository = tripsRepository;
            this.townsDistancesRepository = townsDistancesRepository;
            this.userTripsRepository = userTripsRepository;
        }

        public async Task<string> CreateAsync(TripCreateInputModel inputModel, ApplicationUser user)
        {
            var townsDistance = this.townsDistancesRepository
                .AllAsNoTracking()
                .Where(td => td.Origin == inputModel.Origin && td.Destination == inputModel.Destination)
                .FirstOrDefault();

            if (townsDistance == null)
            {
                return null;
            }

            var trip = new Trip
            {
                DriverId = user.Id,
                TownsDistanceId = townsDistance.Id,
                DateOfDeparture = inputModel.DateOfDeparture,
                TimeOfDeparture = inputModel.TimeOfDeparture,
                TotalSeats = user.Car.PassengerSeats,
                FreeSeats = inputModel.FreeSeats,
                ExpensePerPerson = inputModel.ExpensePerPerson,
                AdditionalInformation = inputModel.AdditionalInformation,
            };

            //var userTrip = new UserTrip
            //{ 
            
            //};

            await this.tripsRepository.AddAsync(trip);
            await this.tripsRepository.SaveChangesAsync();
            //await this.userTripsRepository.AddAsync();

            return trip.Id;
        }
    }
}
