using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Car : BaseEntity
    {
        public int CarId { get; set; }

        [Required(ErrorMessage = "Please input the make!")]
        public string Make { get; set; }

        [Required(ErrorMessage = "Please input the model!")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Please input the number!")]
        public int Inventory { get; set; }
       
        // public List<Rental> Renters { get; set; } = new List<Rental>();
    }
}