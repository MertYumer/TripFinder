namespace TripFinder.Services.Data.Tests
{
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
                UserName = "user@test.com",
                Email = "user@test.com",
                FirstName = "UserFirstName",
                LastName = "UserLastName",
                PhoneNumber = "0880000000",
            };

            this.DbContext.ApplicationUsers.Add(this.User);
            this.DbContext.SaveChanges();

            var car = new Car
            {
                Id = "3c95a1e2-c0fc-468d-abae-8348552a98fc",
                Make = "Ford",
                Model = "Fiesta",
                PassengerSeats = 3,
            };

            this.DbContext.Cars.Add(car);
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
        public async Task UpdateAsyncReturnsNullWhenCarIdDoesNotExist()
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

            Assert.Equal("Ford", car.Make);
            Assert.Equal("Fiesta 1.9 TDI", car.Model);
            Assert.Equal(4, car.PassengerSeats);
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
    }
}
