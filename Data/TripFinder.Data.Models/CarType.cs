using System.ComponentModel.DataAnnotations;

namespace TripFinder.Data.Models
{
    public enum CarType
    {
        [Display(Name = "Hatchback")]
        Hatchback = 1,

        [Display(Name = "Sedan")]
        Sedan = 2,

        [Display(Name = "Combi")]
        Combi = 3,

        [Display(Name = "SUV")]
        SUV = 4,

        [Display(Name = "Coupe")]
        Coupe = 5,

        [Display(Name = "Cabriolet")]
        Cabriolet = 6,

        [Display(Name = "Minivan")]
        Minivan = 7,
    }
}
