namespace TripFinder.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Administration.Dashboard;
    using TripFinder.Web.ViewModels.Cars;
    using Xunit;

    public class CarsServiceTests : BaseServiceTests
    {
        public CarsServiceTests()
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
        public async Task CreateAsyncAddsTheCarInDb()
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
        public async Task CreateAsyncSetCarIdToUserCorrectly()
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
        public async Task DeleteAsyncRemovesTheCarFromTheDb()
        {
            var id = "3c95a1e2-c0fc-468d-abae-8348552a98fc";

            await this.Service.DeleteAsync(id);

            var carsCount = this.DbContext.Cars.Count();

            Assert.Equal(0, carsCount);
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

        [Fact]
        public async Task GenericGetByIdAsyncMapsAllPropertiesCorrectly()
        {
            var id = "3c95a1e2-c0fc-468d-abae-8348552a98fc";

            var viewModel = await this.Service.GetByIdAsync<CarDetailsViewModel>(id);

            Assert.Equal(id, viewModel.Id);
            Assert.Equal("Ford", viewModel.Make);
            Assert.Equal("Fiesta", viewModel.Model);
            Assert.Equal("UserFirstName", viewModel.UserFirstName);
            Assert.Equal("UserLastName", viewModel.UserLastName);
            Assert.Equal(CarType.NotAvailable, viewModel.Type);
            Assert.Equal(Color.None, viewModel.Color);
            Assert.Equal(3, viewModel.PassengerSeats);
            Assert.Null(viewModel.Year);
            Assert.Null(viewModel.ImageId);
            Assert.Null(viewModel.ImageUrl);
            Assert.Null(viewModel.NewImage);
            Assert.False(viewModel.AllowedDrinks);
            Assert.False(viewModel.AllowedFood);
            Assert.False(viewModel.AllowedSmoking);
            Assert.False(viewModel.AllowedPets);
            Assert.False(viewModel.HasAirConditioning);
            Assert.False(viewModel.PlaceForLuggage);
        }

        [Fact]
        public async Task GetByIdAsyncReturnsNullWhenCarDoesNotExist()
        {
            var id = "testId";

            var car = await this.Service.GetByIdAsync(id);

            Assert.Null(car);
        }

        [Fact]
        public async Task GetByIdAsyncReturnsTheCorrectCar()
        {
            var id = "3c95a1e2-c0fc-468d-abae-8348552a98fc";

            var expectedCar = await this.DbContext.Cars.FirstOrDefaultAsync(c => c.Id == id);
            var actualCar = await this.Service.GetByIdAsync(id);

            Assert.StrictEqual(expectedCar, actualCar);
        }

        [Fact]
        public async Task GetAllCarsCountAsyncReturnsTheCorrectCount()
        {
            var carsCount = await this.Service.GetAllCarsCountAsync();

            Assert.Equal(1, carsCount);
        }

        [Fact]
        public async Task GetAllCarsCountAsyncAlsoCountsTheDeletedCars()
        {
            var id = "3c95a1e2-c0fc-468d-abae-8348552a98fc";
            await this.Service.DeleteAsync(id);

            var carsCount = await this.Service.GetAllCarsCountAsync();

            Assert.Equal(1, carsCount);
        }

        [Fact]
        public async Task GetCurrentCarsCountAsyncReturnsTheCorrectCount()
        {
            var carsCount = await this.Service.GetCurrentCarsCountAsync();

            Assert.Equal(1, carsCount);
        }

        [Fact]
        public async Task GetCurrentCarsCountAsyncDoesNotCountTheDeletedCars()
        {
            var id = "3c95a1e2-c0fc-468d-abae-8348552a98fc";
            await this.Service.DeleteAsync(id);

            var carsCount = await this.Service.GetCurrentCarsCountAsync();

            Assert.Equal(0, carsCount);
        }

        [Fact]
        public async Task GetDeletedCarsCountAsyncReturnsTheCorrectCount()
        {
            var carsCount = await this.Service.GetDeletedCarsCountAsync();

            Assert.Equal(0, carsCount);
        }

        [Fact]
        public async Task GetDeletedCarsCountAsyncDoesNotCountTheCurrentCars()
        {
            var carsCount = await this.Service.GetDeletedCarsCountAsync();

            Assert.Equal(0, carsCount);
        }

        [Fact]
        public async Task GetAllCarsAsyncReturnsACollectionFromTheSameType()
        {
            var viewModels = await this.Service.GetAllCarsAsync<CarViewModel>();

            Assert.IsAssignableFrom<IEnumerable<CarViewModel>>(viewModels);
        }

        [Fact]
        public async Task GetAllCarsAsyncReturnsAndDeletedCars()
        {
            var id = "3c95a1e2-c0fc-468d-abae-8348552a98fc";
            await this.Service.DeleteAsync(id);

            var viewModels = await this.Service.GetAllCarsAsync<CarViewModel>();

            Assert.Equal(id, viewModels.FirstOrDefault().Id);
        }

        [Fact]
        public async Task GetDeletedCarDetailsAsyncReturnsTheCorrectCar()
        {
            var id = "3c95a1e2-c0fc-468d-abae-8348552a98fc";

            var expectedCar = await this.Service.GetByIdAsync<CarDetailsViewModel>(id);

            await this.Service.DeleteAsync(id);

            var actualCar = await this.Service.GetDeletedCarDetailsAsync<CarDetailsViewModel>(id);

            Assert.Equal(expectedCar.Id, actualCar.Id);
        }

        [Fact]
        public async Task GetDeletedCarDetailsAsyncReturnsNullWhenTheCarDoesNotExist()
        {
            var id = "testId";

            var viewModel = await this.Service.GetDeletedCarDetailsAsync<CarDetailsViewModel>(id);

            Assert.Null(viewModel);
        }
    }
}
