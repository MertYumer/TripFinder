namespace TripFinder.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Cars;
    using Xunit;

    public class CarsServiceTest : BaseServiceTests
    {
        public CarsServiceTest()
        {
            this.User = new ApplicationUser
            {
                Id = "54475fd2-b87f-4821-9daa-f2d86e6274f6",
                UserName = "user@test.com",
                Email = "user@test.com",
                FirstName = "UserFirstName",
                LastName = "UserLastName",
                PhoneNumber = "0880000000",
            };

            var car = new Car
            {
                Id = "3c95a1e2-c0fc-468d-abae-8348552a98fc",
                Make = "Ford",
                Model = "Fiesta",
                PassengerSeats = 3,
            };

            this.User.Car = car;

            this.DbContext.ApplicationUsers.Add(this.User);
            this.DbContext.SaveChanges();
        }

        private ICarsService Service => this.ServiceProvider.GetRequiredService<ICarsService>();

        private ApplicationUser User { get; set; }

        [Fact]
        public async Task CreateAsyncAddCarInDb()
        {
            var inputModel = new CarCreateInputModel
            {
                Make = "Audi",
                Model = "R8",
                PassengerSeats = 2,
            };

            await this.Service.CreateAsync(inputModel, this.User);
            Assert.Equal(2, this.DbContext.Cars.Count());
        }

        [Fact]
        public async Task CreateAsyncSetCarToUserCorrectly()
        {
            var inputModel = new CarCreateInputModel
            {
                Make = "Audi",
                Model = "R8",
                PassengerSeats = 2,
            };

            var carId = await this.Service.CreateAsync(inputModel, this.User);

            Assert.Equal(carId, this.User.CarId);
        }

        [Fact]
        public async Task UpdateAsyncReturnsNullWhenCarDoesNotExist()
        {
            var inputModel = new CarEditInputModel
            {
                Id = "testId",
                Make = "Audi",
                Model = "R8",
                PassengerSeats = 2,
            };

            var carId = await this.Service.UpdateAsync(inputModel);

            Assert.Null(carId);
        }

        [Fact]
        public async Task UpdateAsyncReturnsTheCarId()
        {
            var inputModel = new CarEditInputModel
            {
                Id = "3c95a1e2-c0fc-468d-abae-8348552a98fc",
                Make = "Ford",
                Model = "Fiesta 1.9 TDI",
                PassengerSeats = 4,
            };

            var carId = await this.Service.UpdateAsync(inputModel);

            Assert.Equal("3c95a1e2-c0fc-468d-abae-8348552a98fc", carId);
        }

        [Fact]
        public async Task UpdateAsyncSetsAllProperiestCorrect()
        {
            var inputModel = new CarEditInputModel
            {
                Id = "3c95a1e2-c0fc-468d-abae-8348552a98fc",
                Make = "Ford",
                Model = "Fiesta 1.9 TDI",
                PassengerSeats = 4,
            };

            var carId = await this.Service.UpdateAsync(inputModel);

            var car = await this.DbContext.Cars.FirstOrDefaultAsync(c => c.Id == carId);

            Assert.Equal(inputModel.Make, car.Make);
            Assert.Equal(inputModel.Model, car.Model);
            Assert.Equal(inputModel.PassengerSeats, car.PassengerSeats);
            Assert.Equal(CarType.NotAvailable, car.Type);
            Assert.Equal(Color.None, car.Color);
            Assert.Null(car.Year);
            Assert.Null(car.ImageId);
            Assert.False(car.AllowedDrinks);
            Assert.False(car.AllowedFood);
            Assert.False(car.AllowedPets);
            Assert.False(car.AllowedSmoking);
            Assert.False(car.HasAirConditioning);
            Assert.False(car.PlaceForLuggage);
        }

        [Fact]
        public async Task DeleteAsyncReturnsNullWhenCarDoesNotExist()
        {
            var id = "testId";

            var carId = await this.Service.DeleteAsync(id);

            Assert.Null(carId);
        }

        [Fact]
        public async Task DeleteAsyncSetsUsersCarIdToNull()
        {
            var user = await this.DbContext.ApplicationUsers.FirstOrDefaultAsync();
            var car = await this.DbContext.Cars.FirstOrDefaultAsync();

            await this.Service.DeleteAsync(car.Id);

            Assert.Null(user.CarId);
        }

        [Fact]
        public async Task DeleteAsyncReturnsTheCarId()
        {
            var car = await this.DbContext.Cars.FirstOrDefaultAsync();

            var carId = await this.Service.DeleteAsync(car.Id);

            Assert.Equal(car.Id, carId);
        }

        [Fact]
        public async Task GenericGetByIdAsyncReturnsNullWhenCarDoesNotExist()
        {
            var id = "testId";

            var viewModel = await this.Service.GetByIdAsync<CarDetailsViewModel>(id);

            Assert.Null(viewModel);
        }

        [Fact]
        public async Task GenericGetByIdAsyncReturnsTheCorrectType()
        {
            var id = "3c95a1e2-c0fc-468d-abae-8348552a98fc";

            var viewModel = await this.Service.GetByIdAsync<CarDetailsViewModel>(id);

            Assert.IsType<CarDetailsViewModel>(viewModel);
        }
    }
}
