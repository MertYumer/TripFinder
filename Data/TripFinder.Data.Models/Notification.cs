namespace TripFinder.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using TripFinder.Data.Common.Models;

    public class Notification : BaseDeletableModel<string>
    {
        public Notification()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedOn = DateTime.UtcNow;
        }

        [Required]
        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public virtual ApplicationUser Receiver { get; set; }

        [Required]
        public string TripId { get; set; }

        public virtual Trip Trip { get; set; }

        [Required]
        public NotificationSubject Subject { get; set; }

        [Required]
        public string ReceiverMessage { get; set; }

        [Required]
        public string SenderMessage { get; set; }
    }
}
