namespace TripFinder.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public enum CarType
    {
        [Display(Name = "N/A")]
        NotAvailable = 0,

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
