namespace TripFinder.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;
    using TripFinder.Web.ViewModels.Cars;

    public class CarsService : ICarsService
    {
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<Car> carsRepository;

        private readonly IImagesService imagesService;

        public CarsService(
            IRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<Car> carsRepository,
            IImagesService imagesService)
        {
            this.usersRepository = usersRepository;
            this.carsRepository = carsRepository;
            this.imagesService = imagesService;
        }

        public async Task<string> CreateAsync(CarCreateInputModel inputModel, ApplicationUser user)
        {
            var car = new Car
            {
                UserId = user.Id,
                Make = inputModel.Make,
                Model = inputModel.Model,
                Type = inputModel.Type,
                Color = inputModel.Color,
                Year = inputModel.Year,
                PassengerSeats = inputModel.PassengerSeats,
                AllowedSmoking = inputModel.AllowedSmoking,
                AllowedDrinks = inputModel.AllowedDrinks,
                AllowedFood = inputModel.AllowedFood,
                AllowedPets = inputModel.AllowedPets,
                HasAirConditioning = inputModel.HasAirConditioning,
                PlaceForLuggage = inputModel.PlaceForLuggage,
            };

            user.Car = car;
            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();

            return user.CarId;
        }

        public async Task<string> UpdateAsync(CarEditInputModel inputModel)
        {
            var car = this.carsRepository
                .All()
                .FirstOrDefault(c => c.Id == inputModel.Id);

            if (car == null)
            {
                return null;
            }

            car.Make = inputModel.Make;
            car.Model = inputModel.Model;
            car.Type = inputModel.Type;
            car.Year = inputModel.Year;
            car.Color = inputModel.Color;
            car.PassengerSeats = inputModel.PassengerSeats;
            car.HasAirConditioning = inputModel.HasAirConditioning;
            car.AllowedDrinks = inputModel.AllowedDrinks;
            car.AllowedFood = inputModel.AllowedFood;
            car.AllowedPets = inputModel.AllowedPets;
            car.AllowedSmoking = inputModel.AllowedSmoking;
            car.PlaceForLuggage = inputModel.PlaceForLuggage;

            if (inputModel.NewImage != null)
            {
                var oldImageId = car.ImageId;
                var newImage = await this.imagesService.CreateAsync(inputModel.NewImage);
                car.ImageId = newImage.Id;
                await this.imagesService.DeleteAsync(oldImageId);
            }

            this.carsRepository.Update(car);
            await this.carsRepository.SaveChangesAsync();

            return car.Id;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var user = this.usersRepository
                .All()
                .Include(u => u.Car)
                .FirstOrDefault(u => u.CarId == id);

            if (user.CarId == null)
            {
                return null;
            }

            var carId = user.CarId;
            user.CarId = null;

            this.carsRepository.Delete(user.Car);
            await this.carsRepository.SaveChangesAsync();

            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();

            return carId;
        }

        public T GetById<T>(string id)
        {
            var car = this.carsRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return car;
        }

        public Car GetById(string id)
        {
            var car = this.carsRepository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            return car;
        }

        public async Task<int> GetAllCarsCountAsync()
        {
            var allCarsCount = await this.carsRepository
                .AllWithDeleted()
                .CountAsync();

            return allCarsCount;
        }

        public async Task<int> GetCurrentCarsCountAsync()
        {
            var currentCarsCount = await this.carsRepository
                .All()
                .CountAsync();

            return currentCarsCount;
        }

        public async Task<int> GetDeletedCarsCountAsync()
        {
            var deletedCarsCount = await this.carsRepository
                .AllWithDeleted()
                .Where(c => c.IsDeleted)
                .CountAsync();

            return deletedCarsCount;
        }
    }
}
