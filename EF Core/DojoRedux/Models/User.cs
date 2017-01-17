using System.Collections.Generic;

namespace QuotingRedux.Models
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Quote> Quotes { get; set; } = new List<Quote>();
    }
}