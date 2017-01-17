using System.ComponentModel.DataAnnotations;

namespace LogReg.Models
{
    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your first name!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name should be letters only!")]
        [MinLength(2, ErrorMessage = "Too short!First name should be at least 2 letters!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name should be letters only!")]
        [MinLength(2, ErrorMessage = "Too short!Last name should be at least 2 letters!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email!")]
        [RegularExpression(@"^[a-zA-Z0-9\.\+_-]+@[a-zA-Z0-9\._-]+\.[a-zA-Z]+$", ErrorMessage="Please enter a valid email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please create a new password!")]
        [RegularExpression(@"^[A-Za-z\d@$!%*?&]+$", ErrorMessage="Please create a valid password as per the criteria!")]
        [MinLength(8, ErrorMessage = "Too short!Passwords must be at least 8 characters.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The passwords entered don't match!")]
        public string Confirm { get; set; }
        
    }
}