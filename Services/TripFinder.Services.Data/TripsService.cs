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

        public TripsService(
            IDeletableEntityRepository<Trip> tripsRepository,
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

        public IEnumerable<T> GetAllTrips<T>()
        {
            var trips = this.tripsRepository
                .All()
                .Include(t => t.Driver)
                .ThenInclude(d => d.AvatarImage)
                .Include(t => t.Car)
                .To<T>();

            return trips;
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

        public async Task DeletePassedTrips()
        {
            var passedTrips = this.tripsRepository
                .All()
                .Include(t => t.TownsDistance)
                .Where(t => t.DateOfDeparture.Date.CompareTo(DateTime.UtcNow.Date) < 0
                || t.DateOfDeparture.Date.CompareTo(DateTime.UtcNow.Date) == 0)
                .ToList();

            foreach (var trip in passedTrips)
            {
                if (trip.DateOfDeparture.Date.CompareTo(DateTime.UtcNow.Date) == 0)
                {
                    if (trip.TimeOfDeparture.TimeOfDay.TotalMinutes + trip.TownsDistance.EstimatedMinutes >
                        DateTime.UtcNow.TimeOfDay.TotalMinutes)
                    {
                        continue;
                    }
                }

                this.tripsRepository.Delete(trip);
            }

            await this.tripsRepository.SaveChangesAsync();
        }
    }
}
