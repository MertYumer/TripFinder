namespace TripFinder.Web.ViewModels.Administration.Dashboard
{
    using System;

    using AutoMapper;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class TripViewModel : IMapFrom<Trip>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public Town Origin { get; set; }

        public Town Destination { get; set; }

        public string Creator { get; set; }

        public string Car { get; set; }

        public int PassengersCount { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime DateOfDeparture { get; set; }

        public DateTime DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Trip, TripViewModel>()
                 .ForMember(vm => vm.Creator, opt => opt.MapFrom(t => $"{t.Driver.FirstName} {t.Driver.LastName}"))
                 .ForMember(vm => vm.Car, opt => opt.MapFrom(t => $"{t.Car.Make} {t.Car.Model}"))
                 .ForMember(vm => vm.PassengersCount, opt => opt.MapFrom(t => t.UserTrips.Count - 1));
        }
    }
}
