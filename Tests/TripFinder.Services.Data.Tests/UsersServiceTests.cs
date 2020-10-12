namespace TripFinder.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using TripFinder.Data.Models;
    using TripFinder.Web.ViewModels.Administration.Dashboard;
    using TripFinder.Web.ViewModels.Users;
    using Xunit;

    public class UsersServiceTests : BaseServiceTests
    {
        public UsersServiceTests()
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

            this.DbContext.ApplicationUsers.Add(this.User);
            this.DbContext.SaveChanges();
        }

        private IUsersService Service => this.ServiceProvider.GetRequiredService<IUsersService>();

        private ApplicationUser User { get; set; }

        [Fact]
        public async Task CheckForUserByIdAsyncReturnsTheUserId()
        {
            var expectedId = this.User.Id;

            var actualId = await this.Service.CheckForUserByIdAsync(expectedId);

            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public async Task CheckForUserByIdAsyncReturnsNullWhenUserDoesNotExist()
        {
            var id = "testId";

            var userId = await this.Service.CheckForUserByIdAsync(id);

            Assert.Null(userId);
        }

        [Fact]
        public async Task DeleteAsyncReturnsTheUserId()
        {
            var id = this.User.Id;

            var userId = await this.Service.DeleteAsync(id);

            Assert.Equal(this.User.Id, userId);
        }

        [Fact]
        public async Task DeleteAsyncReturnsNullWhenUserDoesNotExist()
        {
            var id = "testId";

            var userId = await this.Service.DeleteAsync(id);

            Assert.Null(userId);
        }

        [Fact]
        public async Task DeleteAsyncRemovesTheUserFromTheDb()
        {
            var id = this.User.Id;

            await this.Service.DeleteAsync(id);

            var usersCount = this.DbContext.ApplicationUsers.Count();

            Assert.Equal(0, usersCount);
        }

        [Fact]
        public async Task GenericGetByIdAsyncReturnsNullWhenUserDoesNotExist()
        {
            var id = "testId";

            var viewModel = await this.Service.GetByIdAsync<UserDetailsViewModel>(id);

            Assert.Null(viewModel);
        }

        [Fact]
        public async Task GenericGetByIdAsyncReturnsTheCorrectType()
        {
            var id = this.User.Id;

            var viewModel = await this.Service.GetByIdAsync<UserDetailsViewModel>(id);

            Assert.IsType<UserDetailsViewModel>(viewModel);
        }

        [Fact]
        public async Task GetByIdAsyncReturnsNullWhenCUserDoesNotExist()
        {
            var id = "testId";

            var user = await this.Service.GetByIdAsync(id);

            Assert.Null(user);
        }

        [Fact]
        public async Task GetByIdAsyncReturnsTheCorrectUser()
        {
            var id = this.User.Id;

            var expectedUser = this.User;
            var actualUser = await this.Service.GetByIdAsync(id);

            Assert.StrictEqual(expectedUser, actualUser);
        }

        [Fact]
        public async Task UpdateAsyncReturnsNullWhenUserDoesNotExist()
        {
            var inputModel = new UserEditInputModel
            {
                Id = "testId",
                Email = this.User.Email,
                FirstName = this.User.FirstName,
                LastName = this.User.LastName,
                PhoneNumber = "0881111111",
            };

            var userId = await this.Service.UpdateAsync(inputModel);

            Assert.Null(userId);
        }

        [Fact]
        public async Task UpdateAsyncReturnsTheUserId()
        {
            var inputModel = new UserEditInputModel
            {
                Id = this.User.Id,
                Email = this.User.Email,
                FirstName = this.User.FirstName,
                LastName = this.User.LastName,
                PhoneNumber = "0881111111",
            };

            var userId = await this.Service.UpdateAsync(inputModel);

            Assert.Equal(this.User.Id, userId);
        }

        [Fact]
        public async Task UpdateAsyncSetsAllProperiestCorrect()
        {
            var inputModel = new UserEditInputModel
            {
                Id = this.User.Id,
                Email = this.User.Email,
                FirstName = this.User.FirstName,
                LastName = this.User.LastName,
                PhoneNumber = "0881111111",
            };

            var userId = await this.Service.UpdateAsync(inputModel);

            var user = await this.DbContext.ApplicationUsers.FirstOrDefaultAsync(c => c.Id == userId);

            Assert.Equal(inputModel.Email, user.Email);
            Assert.Equal(inputModel.FirstName, user.FirstName);
            Assert.Equal(inputModel.LastName, user.LastName);
            Assert.Equal(inputModel.PhoneNumber, user.PhoneNumber);
            Assert.Null(user.Age);
            Assert.Null(user.Gender);
            Assert.Null(user.AvatarImageId);
            Assert.Null(user.AvatarImage);
            Assert.Null(user.CarId);
            Assert.Null(user.Car);
            Assert.False(user.HasUsersToReview);
            Assert.Equal(0, user.TripsCountAsDriver);
            Assert.Equal(0, user.TripsCountAsPassenger);
            Assert.Equal(0, user.Rating);
            Assert.Equal(0, user.RatingsCount);
            Assert.Equal(0, user.TravelledDistance);
        }

        [Fact]
        public async Task UpdateUpdateTripUsersAsyncUpdateAllUsersDetails()
        {
            var driverId = this.User.Id;

            var passenger = new ApplicationUser
            {
                Id = "2f9c1ce9-5d9d-4648-bfe0-1562c559e822",
                UserName = "passenger@test.com",
                Email = "passenger@test.com",
                FirstName = "PassengerFirstName",
                LastName = "PassengerLastName",
                PhoneNumber = "0882222222",
            };

            await this.DbContext.ApplicationUsers.AddAsync(passenger);
            await this.DbContext.SaveChangesAsync();

            var passengersIds = new HashSet<string> { passenger.Id, };
            var distanse = 100;

            await this.Service.UpdateTripUsersAsync(driverId, passengersIds, distanse);

            var driver = await this.DbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == driverId);

            Assert.Equal(1, driver.TripsCountAsDriver);
            Assert.Equal(100, driver.TravelledDistance);
            Assert.True(driver.HasUsersToReview);

            var users = await this.DbContext.ApplicationUsers.Where(u => u.Id != driverId).ToListAsync();

            foreach (var user in users)
            {
                Assert.Equal(1, user.TripsCountAsPassenger);
                Assert.Equal(100, user.TravelledDistance);
                Assert.True(user.HasUsersToReview);
            }
        }

        [Fact]
        public async Task GetAllUsersCountAsyncReturnsTheCorrectCount()
        {
            var usersCount = await this.Service.GetAllUsersCountAsync();

            Assert.Equal(1, usersCount);
        }

        [Fact]
        public async Task GetAllUsersCountAsyncAlsoCountsTheDeletedUsers()
        {
            var id = this.User.Id;
            await this.Service.DeleteAsync(id);

            var usersCount = await this.Service.GetAllUsersCountAsync();

            Assert.Equal(1, usersCount);
        }

        [Fact]
        public async Task GetActiveUsersCountAsyncReturnsTheCorrectCount()
        {
            var usersCount = await this.Service.GetActiveUsersCountAsync();

            Assert.Equal(1, usersCount);
        }

        [Fact]
        public async Task GetActiveUsersCountAsyncDoesNotCountTheDeletedUsers()
        {
            var id = this.User.Id;
            await this.Service.DeleteAsync(id);

            var usersCount = await this.Service.GetActiveUsersCountAsync();

            Assert.Equal(0, usersCount);
        }

        [Fact]
        public async Task GetDeletedUsersCountAsyncReturnsTheCorrectCount()
        {
            var usersCount = await this.Service.GetDeletedUsersCountAsync();

            Assert.Equal(0, usersCount);
        }

        [Fact]
        public async Task GetDeletedUsersCountAsyncDoesNotCountTheActiveUsers()
        {
            var usersCount = await this.Service.GetDeletedUsersCountAsync();

            Assert.Equal(0, usersCount);
        }

        [Fact]
        public async Task GetAllUsersAsyncReturnsACollectionFromTheSameType()
        {
            var viewModels = await this.Service.GetAllUsersAsync<UserViewModel>();

            Assert.IsAssignableFrom<IEnumerable<UserViewModel>>(viewModels);
        }

        [Fact]
        public async Task GetAllUsersAsyncReturnsAndDeletedUsers()
        {
            var id = this.User.Id;
            await this.Service.DeleteAsync(id);

            var viewModels = await this.Service.GetAllUsersAsync<UserViewModel>();

            Assert.Equal(id, viewModels.FirstOrDefault().Id);
        }

        [Fact]
        public async Task GetDeletedUserDetailsAsyncReturnsTheCorrectUser()
        {
            var id = this.User.Id;

            var expectedUser = await this.Service.GetByIdAsync<UserDetailsViewModel>(id);

            await this.Service.DeleteAsync(id);

            var actualUser = await this.Service.GetDeletedUserDetailsAsync<UserDetailsViewModel>(id);

            Assert.Equal(expectedUser.Id, actualUser.Id);
        }

        [Fact]
        public async Task GetDeletedCarDetailsAsyncReturnsNullWhenTheUserDoesNotExist()
        {
            var id = "testId";

            var viewModel = await this.Service.GetDeletedUserDetailsAsync<UserDetailsViewModel>(id);

            Assert.Null(viewModel);
        }
    }
}
