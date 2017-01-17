using System.ComponentModel.DataAnnotations;

namespace quotingDojo.Models
{
    public abstract class BaseEntity {}
    public class Quotes : BaseEntity
    {
        [Required(ErrorMessage = "Please enter your name!")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Please enter a valid name!")]
        [MinLength(2, ErrorMessage = "Name is too short!")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Please enter your quote!")]
        public string Content { get; set; }

        public int Likes { get; set; }
        public int Id { get; set; }
        public string Date { get; set; }
    }
}