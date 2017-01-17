using System;

namespace Wall.Models
{
    public class Comment : BaseEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Created_at { get; set; }
        public int Message_ID { get; set; }
        public User Author { get; set; }
    }
}