namespace TripFinder.Web.ViewModels.Administration.Dashboard
{
    using System;

    using AutoMapper;
    using TripFinder.Data.Models;
    using TripFinder.Services.Mapping;

    public class CarViewModel : IMapFrom<Car>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Owner { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Car, CarViewModel>()
                 .ForMember(vm => vm.Owner, opt => opt.MapFrom(c => $"{c.User.FirstName} {c.User.LastName}"));
        }
    }
}
