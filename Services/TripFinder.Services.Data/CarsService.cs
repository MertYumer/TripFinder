namespace TripFinder.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;
    using TripFinder.Web.ViewModels.Cars;

    public class CarsService : ICarsService
    {
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IRepository<Car> carsRepository;
        private readonly IImagesService imagesService;

        public CarsService(
            IRepository<ApplicationUser> usersRepository,
            IRepository<Car> carsRepository,
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

        public T GetById<T>(string id)
        {
            var car = this.carsRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return car;
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

            if (inputModel.NewImage == null)
            {
                this.carsRepository.Update(car);
                await this.carsRepository.SaveChangesAsync();
            }
            else
            {
                var oldImageId = car.ImageId;
                var newImage = await this.imagesService.CreateAsync(inputModel.NewImage);
                car.ImageId = newImage.Id;

                this.carsRepository.Update(car);
                await this.carsRepository.SaveChangesAsync();

                await this.imagesService.DeleteAsync(oldImageId);
            }

            return car.Id;
        }
    }
}
