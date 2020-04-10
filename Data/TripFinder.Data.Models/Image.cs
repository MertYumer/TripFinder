namespace TripFinder.Data.Models
{
    using System;

    using TripFinder.Data.Common.Models;

    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
        }

        public string Url { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string CarId { get; set; }

        public virtual Car Car { get; set; }
    }
}
