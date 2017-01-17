using System.Collections.Generic;

namespace CarRental.Models
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<Rental> Rents { get; set; } = new List<Rental>();
    }
}