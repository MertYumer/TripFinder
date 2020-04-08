namespace TripFinder.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;
    using TripFinder.Web.ViewModels.Trips;

    public class TripsService : ITripsService
    {
        private readonly IDeletableEntityRepository<Trip> tripsRepository;
        private readonly IRepository<TownsDistance> townsDistancesRepository;
        private readonly IRepository<UserTrip> userTripsRepository;

        public TripsService(
            IDeletableEntityRepository<Trip> tripsRepository,
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
                .All()
                .Where(td => (td.Origin == inputModel.Origin && td.Destination == inputModel.Destination)
                || (td.Origin == inputModel.Destination && td.Destination == inputModel.Origin))
                .FirstOrDefault();

            if (townsDistance == null)
            {
                return null;
            }

            if (inputModel.FreeSeats > user.Car.PassengerSeats)
            {
                return null;
            }

            var trip = new Trip
            {
                DriverId = user.Id,
                CarId = user.CarId,
                TownsDistanceId = townsDistance.Id,
                Origin = inputModel.Origin,
                Destination = inputModel.Destination,
                DateOfDeparture = inputModel.DateOfDeparture,
                TimeOfDeparture = inputModel.TimeOfDeparture,
                TotalSeats = user.Car.PassengerSeats,
                FreeSeats = inputModel.FreeSeats,
                ExpensePerPerson = inputModel.ExpensePerPerson,
                AdditionalInformation = inputModel.AdditionalInformation,
            };

            await this.tripsRepository.AddAsync(trip);
            await this.tripsRepository.SaveChangesAsync();

            var userTrip = new UserTrip
            {
                UserId = user.Id,
                TripId = trip.Id,
            };

            await this.userTripsRepository.AddAsync(userTrip);
            await this.userTripsRepository.SaveChangesAsync();

            return trip.Id;
        }

        public T GetById<T>(string id)
        {
            var trip = this.tripsRepository
                .All()
                .Include(t => t.Driver)
                .ThenInclude(d => d.AvatarImage)
                .Include(t => t.Car)
                .ThenInclude(c => c.Image)
                .Where(t => t.Id == id)
                .To<T>()
                .FirstOrDefault();

            return trip;
        }

        public async Task UpdateTripViewsCountAsync(string id)
        {
            var trip = this.tripsRepository
                .All()
                .FirstOrDefault(t => t.Id == id);

            trip.Views++;

            this.tripsRepository.Update(trip);
            await this.tripsRepository.SaveChangesAsync();
        }

        public async Task DeletePassedTripsAsync()
        {
            var passedTrips = this.tripsRepository
                .All()
                .Include(t => t.TownsDistance)
                .Where(t => t.DateOfDeparture.Date.CompareTo(DateTime.Now.Date) < 0
                || t.DateOfDeparture.Date.CompareTo(DateTime.Now.Date) == 0)
                .ToList();

            foreach (var trip in passedTrips)
            {
                if (trip.DateOfDeparture.Date.CompareTo(DateTime.Now.Date) == 0)
                {
                    if (trip.TimeOfDeparture.TimeOfDay.TotalMinutes + trip.TownsDistance.EstimatedMinutes >
                        DateTime.Now.TimeOfDay.TotalMinutes)
                    {
                        continue;
                    }
                }

                this.tripsRepository.Delete(trip);
            }

            await this.tripsRepository.SaveChangesAsync();
        }

        public async Task<string> UpdateAsync(TripEditInputModel inputModel)
        {
            var trip = this.tripsRepository
                .All()
                .FirstOrDefault(t => t.Id == inputModel.Id);

            if (trip == null)
            {
                return null;
            }

            trip.DateOfDeparture = inputModel.DateOfDeparture;
            trip.TimeOfDeparture = inputModel.TimeOfDeparture;
            trip.FreeSeats = inputModel.FreeSeats;
            trip.ExpensePerPerson = inputModel.ExpensePerPerson;
            trip.AdditionalInformation = inputModel.AdditionalInformation;

            this.tripsRepository.Update(trip);
            await this.tripsRepository.SaveChangesAsync();

            return trip.Id;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var trip = this.tripsRepository
                .All()
                .FirstOrDefault(t => t.Id == id);

            if (trip == null)
            {
                return null;
            }

            var tripId = trip.Id;

            this.tripsRepository.Delete(trip);
            await this.tripsRepository.SaveChangesAsync();

            return tripId;
        }

        public IEnumerable<T> GetAllTrips<T>(int? take = null, int skip = 0)
        {
            var query = this.tripsRepository
                .All()
                .Include(t => t.Driver)
                .ThenInclude(d => d.AvatarImage)
                .Include(t => t.Car)
                .OrderByDescending(t => t.CreatedOn)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            var trips = query
                .To<T>()
                .ToList();

            return trips;
        }

        public IEnumerable<T> GetMyTrips<T>(string userId, int? take = null, int skip = 0)
        {
            var query = this.tripsRepository
                .All()
                .Include(t => t.Driver)
                .ThenInclude(d => d.AvatarImage)
                .Include(t => t.Car)
                .Include(t => t.UserTrips)
                .Where(t => t.UserTrips.Any(ut => ut.UserId == userId))
                .OrderByDescending(t => t.CreatedOn)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            var trips = query
                .To<T>()
                .ToList();

            return trips;
        }

        public IEnumerable<T> ShowSearchResults<T>(TripSearchInputModel inputModel, string userId, int? take = null, int skip = 0)
        {
            var townsDistance = this.townsDistancesRepository
                .All()
                .Where(td => (td.Origin == inputModel.Origin && td.Destination == inputModel.Destination)
                || (td.Origin == inputModel.Destination && td.Destination == inputModel.Origin))
                .FirstOrDefault();

            if (townsDistance == null)
            {
                return null;
            }

            var query = this.tripsRepository
                .All()
                .Include(t => t.Driver)
                .ThenInclude(d => d.AvatarImage)
                .Include(t => t.Car)
                .Include(t => t.TownsDistance)
                .Where(t => t.UserTrips.All(ut => ut.UserId != userId)
                && t.Origin == inputModel.Origin
                && t.Destination == inputModel.Destination
                && t.DateOfDeparture.Date.CompareTo(inputModel.DateOfDeparture) == 0
                && t.FreeSeats >= inputModel.SeatsNeeded)
                .OrderBy(t => t.DateOfDeparture)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            var trips = query
                .To<T>()
                .ToList();

            return trips;
        }

        public int GetAllTripsCount()
        {
            var tripsCount = this.tripsRepository
                .All()
                .ToList()
                .Count;

            return tripsCount;
        }

        public int GetMyTripsCount(string userId)
        {
            var tripsCount = this.tripsRepository
                .All()
                .Include(t => t.UserTrips)
                .Where(t => t.UserTrips.Any(ut => ut.UserId == userId))
                .ToList()
                .Count;

            return tripsCount;
        }

        public int GetSearchResultsCount(TripSearchInputModel inputModel, string userId)
        {
            var townsDistance = this.townsDistancesRepository
                .All()
                .Where(td => (td.Origin == inputModel.Origin && td.Destination == inputModel.Destination)
                || (td.Origin == inputModel.Destination && td.Destination == inputModel.Origin))
                .FirstOrDefault();

            var query = this.tripsRepository
                .All()
                .Include(t => t.Driver)
                .ThenInclude(d => d.AvatarImage)
                .Include(t => t.Car)
                .Include(t => t.TownsDistance)
                .Where(t => t.UserTrips.All(ut => ut.UserId != userId)
                && t.Origin == inputModel.Origin
                && t.Destination == inputModel.Destination
                && t.DateOfDeparture.Date.CompareTo(inputModel.DateOfDeparture) == 0
                && t.FreeSeats >= inputModel.SeatsNeeded)
                .ToList()
                .Count;

            var tripsCount = this.tripsRepository
                .All()
                .Include(t => t.UserTrips)
                .Where(t => t.UserTrips.Any(ut => ut.UserId == userId))
                .ToList()
                .Count;

            return tripsCount;
        }
    }
}
