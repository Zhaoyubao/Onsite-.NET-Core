using System;

namespace CarRental.Models
{
    public class Rental : BaseEntity
    {
        public int RentalId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public DateTime CheckDate { get; set; } = DateTime.Now;
        public DateTime ReturnDate { get; set; }
    }
}