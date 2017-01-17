using System.ComponentModel.DataAnnotations;

namespace QuotingRedux.Models
{
    public class QuoteViewModel
    {
        
        [Required(ErrorMessage = "Please enter your quote!")]
        public string Content { get; set; }
    }
}