namespace TripFinder.Services.Data.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

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
            Assert.Equal(1, this.DbContext.Cars.Count());
        }
    }
}
