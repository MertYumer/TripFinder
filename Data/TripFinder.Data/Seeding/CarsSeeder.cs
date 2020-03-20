namespace TripFinder.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TripFinder.Data.Models;

    public class CarsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Cars.Any())
            {
                return;
            }

            var cars = new List<Car>()
            {
                new Car { Make = "Audi", Model = "RS6", PassengerSeats = 3, ApplicationUserId = dbContext.ApplicationUsers.FirstOrDefault().Id, AllowedSmoking = false, AllowedFood = false, AllowedDrinks = true, AllowedPets = false, PlaceForLuggage = true },
                new Car { Make = "BMW", Model = "M5 Competition", PassengerSeats = 3, ApplicationUserId = dbContext.ApplicationUsers.Skip(1).FirstOrDefault().Id, AllowedSmoking = false, AllowedFood = true, AllowedDrinks = true, AllowedPets = false, PlaceForLuggage = true },
                new Car { Make = "Mercedes", Model = "AMG C63", PassengerSeats = 3, ApplicationUserId = dbContext.ApplicationUsers.Skip(2).FirstOrDefault().Id, AllowedSmoking = true, AllowedFood = false, AllowedDrinks = true, AllowedPets = false, PlaceForLuggage = false },
                new Car { Make = "Porsche", Model = "Panamera", PassengerSeats = 3, ApplicationUserId = dbContext.ApplicationUsers.Skip(3).FirstOrDefault().Id, AllowedSmoking = false, AllowedFood = false, AllowedDrinks = true, AllowedPets = true, PlaceForLuggage = false },
            };

            await dbContext.Cars.AddRangeAsync(cars);
            await dbContext.SaveChangesAsync();
        }
    }
}
