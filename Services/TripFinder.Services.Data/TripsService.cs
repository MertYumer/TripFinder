﻿namespace TripFinder.Services.Data
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

        private readonly IUsersService usersService;

        public TripsService(
            IDeletableEntityRepository<Trip> tripsRepository,
            IRepository<TownsDistance> townsDistancesRepository,
            IRepository<UserTrip> userTripsRepository,
            IUsersService usersService)
        {
            this.tripsRepository = tripsRepository;
            this.townsDistancesRepository = townsDistancesRepository;
            this.userTripsRepository = userTripsRepository;
            this.usersService = usersService;
        }

        public async Task<string> CreateAsync(TripCreateInputModel inputModel, ApplicationUser user)
        {
            var townsDistanceId = await this.GetOriginAndDestination(inputModel.Origin, inputModel.Destination);

            if (townsDistanceId == null)
            {
                return null;
            }

            if (inputModel.FreeSeats > user.Car.PassengerSeats)
            {
                return null;
            }

            var origin = (Town)Enum.Parse(typeof(Town), inputModel.Origin.Replace(" ", string.Empty));
            var destination = (Town)Enum.Parse(typeof(Town), inputModel.Destination.Replace(" ", string.Empty));

            var trip = new Trip
            {
                DriverId = user.Id,
                CarId = user.CarId,
                TownsDistanceId = townsDistanceId,
                Origin = origin,
                Destination = destination,
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

        public async Task<T> GetByIdAsync<T>(string id)
        {
            var trip = await this.tripsRepository
                .All()
                .Include(t => t.Driver)
                .Include(t => t.Car)
                .Include(t => t.UserTrips)
                .ThenInclude(ut => ut.User)
                .Where(t => t.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return trip;
        }

        public async Task<Trip> GetByIdAsync(string id)
        {
            var trip = await this.tripsRepository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

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

        public async Task<string> UpdateAsync(TripEditInputModel inputModel)
        {
            var trip = await this.tripsRepository
                .All()
                .FirstOrDefaultAsync(t => t.Id == inputModel.Id);

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
            var trip = await this.tripsRepository
                .All()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null)
            {
                return null;
            }

            var tripId = trip.Id;

            this.tripsRepository.Delete(trip);
            await this.tripsRepository.SaveChangesAsync();

            return tripId;
        }

        public async Task<IEnumerable<T>> GetAllTripsAsync<T>(int take, int skip = 0)
        {
            var trips = await this.tripsRepository
                .All()
                .Include(t => t.Driver)
                .ThenInclude(d => d.AvatarImage)
                .Include(t => t.Car)
                .OrderByDescending(t => t.CreatedOn)
                .Skip(skip)
                .Take(take)
                .To<T>()
                .ToListAsync();

            return trips;
        }

        public async Task<IEnumerable<T>> GetMyTripsAsync<T>(string userId, int take, int skip = 0)
        {
            var trips = await this.tripsRepository
                .All()
                .Include(t => t.Driver)
                .ThenInclude(d => d.AvatarImage)
                .Include(t => t.Car)
                .Include(t => t.UserTrips)
                .Where(t => t.UserTrips.Any(ut => ut.UserId == userId))
                .OrderByDescending(t => t.CreatedOn)
                .Skip(skip)
                .Take(take)
                .To<T>()
                .ToListAsync();

            return trips;
        }

        public async Task<IEnumerable<T>> ShowSearchResultsAsync<T>(TripSearchInputModel inputModel, string userId, int take, int skip = 0)
        {
            var townsDistanceId = await this.GetOriginAndDestination(inputModel.Origin, inputModel.Destination);

            if (townsDistanceId == null)
            {
                return null;
            }

            var origin = (Town)Enum.Parse(typeof(Town), inputModel.Origin.Replace(" ", string.Empty));
            var destination = (Town)Enum.Parse(typeof(Town), inputModel.Destination.Replace(" ", string.Empty));

            var trips = await this.tripsRepository
                .All()
                .Include(t => t.Driver)
                .ThenInclude(d => d.AvatarImage)
                .Include(t => t.Car)
                .Include(t => t.TownsDistance)
                .Where(t => t.UserTrips.All(ut => ut.UserId != userId)
                && t.Origin == origin
                && t.Destination == destination
                && t.DateOfDeparture.Date.CompareTo(inputModel.DateOfDeparture) == 0
                && t.FreeSeats >= inputModel.SeatsNeeded)
                .OrderBy(t => t.DateOfDeparture)
                .Skip(skip)
                .Take(take)
                .To<T>()
                .ToListAsync();

            return trips;
        }

        public async Task<int> GetMyTripsCountAsync(string userId)
        {
            var tripsCount = await this.tripsRepository
                .All()
                .Include(t => t.UserTrips)
                .Where(t => t.UserTrips.Any(ut => ut.UserId == userId))
                .CountAsync();

            return tripsCount;
        }

        public async Task<int> GetSearchResultsCountAsync(TripSearchInputModel inputModel, string userId)
        {
            var origin = (Town)Enum.Parse(typeof(Town), inputModel.Origin.Replace(" ", string.Empty));
            var destination = (Town)Enum.Parse(typeof(Town), inputModel.Destination.Replace(" ", string.Empty));

            var tripsCount = await this.tripsRepository
                .All()
                .Include(t => t.Driver)
                .ThenInclude(d => d.AvatarImage)
                .Include(t => t.Car)
                .Include(t => t.TownsDistance)
                .Where(t => t.UserTrips.All(ut => ut.UserId != userId)
                && t.Origin == origin
                && t.Destination == destination
                && t.DateOfDeparture.Date.CompareTo(inputModel.DateOfDeparture) == 0
                && t.FreeSeats >= inputModel.SeatsNeeded)
                .CountAsync();

            return tripsCount;
        }

        public async Task<string> AddUserToTripAsync(string requestorId, string tripCreatorId, string tripId)
        {
            var trip = await this.GetByIdAsync(tripId);
            var requestor = await this.usersService.GetByIdAsync(requestorId);
            var tripCreator = await this.usersService.GetByIdAsync(tripCreatorId);

            if (requestor == null || tripCreator == null || trip == null)
            {
                return null;
            }

            if (trip.DriverId != tripCreator.Id || trip.FreeSeats == 0)
            {
                return null;
            }

            var userTrip = await this.userTripsRepository
                .All()
                .Where(ut => ut.UserId == requestorId && ut.TripId == tripId)
                .FirstOrDefaultAsync();

            if (userTrip != null)
            {
                return null;
            }

            userTrip = new UserTrip
            {
                UserId = requestor.Id,
                TripId = trip.Id,
            };

            await this.userTripsRepository.AddAsync(userTrip);
            await this.userTripsRepository.SaveChangesAsync();

            trip.FreeSeats--;

            this.tripsRepository.Update(trip);
            await this.tripsRepository.SaveChangesAsync();

            return trip.Id;
        }

        public async Task<bool> CheckForUserTripAsync(string userId, string tripId)
        {
            var userTripExists = await this.userTripsRepository
                .All()
                .AnyAsync(ut => ut.UserId == userId && ut.TripId == tripId);

            return userTripExists;
        }

        public async Task<string> CompleteAsync(string tripId, string userId)
        {
            var trip = await this.tripsRepository
                .All()
                .Include(t => t.TownsDistance)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
            {
                return null;
            }

            if (trip.DateOfDeparture.Date.CompareTo(DateTime.Now.Date) > 0
                || (trip.DateOfDeparture.Date.CompareTo(DateTime.Now.Date) == 0
                && trip.TimeOfDeparture.TimeOfDay.TotalMinutes >= DateTime.Now.TimeOfDay.TotalMinutes))
            {
                return null;
            }

            var usersIds = this.GetDriverAndPassengersIds(trip.Id).ToList();

            if (usersIds == null)
            {
                return null;
            }

            var driverId = usersIds.FirstOrDefault(u => u == trip.DriverId);

            if (driverId != userId)
            {
                return null;
            }

            usersIds.Remove(driverId);

            var updatedUsersCount = await this.usersService
                .UpdateTripUsersAsync(driverId, usersIds, trip.TownsDistance.Distance);

            tripId = await this.DeleteAsync(trip.Id);

            return tripId;
        }

        public IEnumerable<string> GetDriverAndPassengersIds(string id)
        {
            var usersIds = this.tripsRepository
                .All()
                .Include(t => t.UserTrips)
                .FirstOrDefault(t => t.Id == id)
                .UserTrips
                .Select(ut => ut.UserId)
                .ToList();

            if (usersIds == null)
            {
                return null;
            }

            return usersIds;
        }

        public async Task<int> GetAllTripsCountAsync()
        {
            var tripsCount = await this.tripsRepository
                .All()
                .CountAsync();

            return tripsCount;
        }

        public async Task<int> GetAllTripsCountWithDeletedAsync()
        {
            var allTripsCountWithDeleted = await this.tripsRepository
                .AllWithDeleted()
                .CountAsync();

            return allTripsCountWithDeleted;
        }

        public async Task<int> GetActiveTripsCountAsync()
        {
            var activeTripsCount = await this.tripsRepository
                .All()
                .CountAsync();

            return activeTripsCount;
        }

        public async Task<int> GetDeletedTripsCountAsync()
        {
            var deletedTripsCount = await this.tripsRepository
                .AllWithDeleted()
                .Where(t => t.IsDeleted)
                .CountAsync();

            return deletedTripsCount;
        }

        public async Task<IEnumerable<T>> GetAllTripsWithDeletedAsync<T>()
        {
            var allTrips = await this.tripsRepository
                .AllWithDeleted()
                .Where(c => c.Driver.Email != "admin@tripfinder.com")
                .To<T>()
                .ToListAsync();

            return allTrips;
        }

        public async Task<T> GetDeletedTripDetailsAsync<T>(string id)
        {
            var trip = await this.tripsRepository
                .AllWithDeleted()
                .Include(t => t.UserTrips)
                .ThenInclude(ut => ut.User)
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return trip;
        }

        private async Task<string> GetOriginAndDestination(string origin, string destination)
        {
            var townsDistance = await this.townsDistancesRepository
                .All()
                .Where(td => (td.Origin == origin && td.Destination == destination)
                || (td.Origin == destination && td.Destination == origin))
                .FirstOrDefaultAsync();

            if (townsDistance == null)
            {
                return null;
            }

            return townsDistance.Id;
        }
    }
}
