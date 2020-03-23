namespace TripFinder.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using TripFinder.Data.Common.Repositories;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;
    using TripFinder.Web.ViewModels.Cars;

    public class CarsService : ICarsService
    {
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IRepository<Car> carsRepository;

        public CarsService(IRepository<ApplicationUser> usersRepository, IRepository<Car> carsRepository)
        {
            this.usersRepository = usersRepository;
            this.carsRepository = carsRepository;
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
    }
}
