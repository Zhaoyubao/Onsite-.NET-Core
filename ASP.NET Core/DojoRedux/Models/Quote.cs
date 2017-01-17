using System.ComponentModel.DataAnnotations;
using System;

namespace QuotingRedux.Models
{
    public class Quote : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Please enter your quote!")]
        public string Content { get; set; }
        public int Likes { get; set; }
        public string Date { get; set; }
        public DateTime Created_at { get; set; }
        public User Author { get; set; }
    }
}