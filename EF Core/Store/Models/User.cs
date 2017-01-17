using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Store.Models
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please input your username!")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Username should contain only letters(include space)!")]
        [MinLength(2, ErrorMessage = "Too short!Username should be at least 2 letters!")]
        [MaxLength(20, ErrorMessage = "Too long!Username should be no more than 20 letters!")]
        public string Name { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}