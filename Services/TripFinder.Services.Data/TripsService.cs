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

        public TripsService(
            IRepository<Trip> tripsRepository,
            IRepository<TownsDistance> townsDistancesRepository)
        {
            this.tripsRepository = tripsRepository;
            this.townsDistancesRepository = townsDistancesRepository;
        }

        public async Task<string> CreateAsync(TripCreateInputModel inputModel, ApplicationUser user)
        {
            var townsDistance = this.townsDistancesRepository
                .AllAsNoTracking()
                .Where(td => (td.Origin == inputModel.Origin && td.Destination == inputModel.Destination)
                || (td.Origin == inputModel.Destination && td.Destination == inputModel.Origin))
                .FirstOrDefault();

            if (townsDistance == null)
            {
                return null;
            }

            var trip = new Trip
            {
                DriverId = user.Id,
                CarId = user.CarId,
                TownsDistanceId = townsDistance.Id,
                DateOfDeparture = inputModel.DateOfDeparture,
                TimeOfDeparture = inputModel.TimeOfDeparture,
                TotalSeats = user.Car.PassengerSeats,
                FreeSeats = inputModel.FreeSeats,
                ExpensePerPerson = inputModel.ExpensePerPerson,
                AdditionalInformation = inputModel.AdditionalInformation,
            };

            await this.tripsRepository.AddAsync(trip);
            await this.tripsRepository.SaveChangesAsync();

            return trip.Id;
        }
    }
}
