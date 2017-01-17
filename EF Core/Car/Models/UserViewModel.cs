using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Please enter your name!")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name should be letters only!")]
        [MinLength(2, ErrorMessage = "Too short! Name should be at least 2 letters!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter your username!")]
        [RegularExpression(@"^[\w\s]+$", ErrorMessage = "Username is invalid!")]
        [MinLength(2, ErrorMessage = "Too short! Username should be at least 2 letters!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please create a new password!")]
        [RegularExpression(@"^[A-Za-z\d@$!%*?&]+$", ErrorMessage="Please create a valid password as per the criteria!")]
        [MinLength(8, ErrorMessage = "Too short!Passwords must be at least 8 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The passwords entered don't match!")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
    }
}