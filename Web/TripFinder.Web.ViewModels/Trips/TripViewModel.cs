namespace TripFinder.Web.ViewModels.Trips
{
    using System;

    using AutoMapper;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class TripViewModel : IMapFrom<Trip>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string DriverFirstName { get; set; }

        public string DriverAvatarImageUrl { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public string CarMake { get; set; }

        public string CarModel { get; set; }

        public DateTime DateOfDeparture { get; set; }

        public DateTime TimeOfDeparture { get; set; }

        public int FreeSeats { get; set; }

        public int TotalSeats { get; set; }

        public decimal ExpensePerPerson { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Trip, TripViewModel>()
                 .ForMember(vm => vm.Origin, opt => opt.MapFrom(t => t.TownsDistance.Origin))
                 .ForMember(vm => vm.Destination, opt => opt.MapFrom(t => t.TownsDistance.Destination));
        }
    }
}
