namespace TripFinder.Web.ViewModels.Trips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class TripDetailsViewModel : IMapFrom<Trip>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public ApplicationUser Driver { get; set; }

        public string DriverAvatarImageUrl { get; set; }

        public Car Car { get; set; }

        public string CarImageUrl { get; set; }

        public Town Origin { get; set; }

        public Town Destination { get; set; }

        public int Distance { get; set; }

        public int EstimatedMinutes { get; set; }

        public DateTime DateOfDeparture { get; set; }

        public DateTime TimeOfDeparture { get; set; }

        public int FreeSeats { get; set; }

        public int TotalSeats { get; set; }

        public int Views { get; set; }

        public decimal ExpensePerPerson { get; set; }

        public string AdditionalInformation { get; set; }

        public IEnumerable<PassengerViewModel> Passengers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Trip, TripDetailsViewModel>()
                 .ForMember(vm => vm.Distance, opt => opt.MapFrom(t => t.TownsDistance.Distance))
                 .ForMember(vm => vm.EstimatedMinutes, opt => opt.MapFrom(t => t.TownsDistance.EstimatedMinutes))
                 .ForMember(vm => vm.Passengers, opt => opt.MapFrom(t => t.UserTrips.Select(ut => ut.User)));
        }
    }
}
