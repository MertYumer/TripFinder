namespace TripFinder.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using TripFinder.Data.Common.Models;

    public class TownsDistance : BaseModel<string>
    {
        public TownsDistance()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
        }

        [Required]
        public string Origin { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public string Distance { get; set; }

        [Required]
        public string EstimatedTime { get; set; }
    }
}
