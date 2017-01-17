using System;

namespace QuotingRedux.Models
{
    public class Quote : BaseEntity
    {
        public int QuoteId { get; set; }
        public string Content { get; set; }
        public int Likes { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}