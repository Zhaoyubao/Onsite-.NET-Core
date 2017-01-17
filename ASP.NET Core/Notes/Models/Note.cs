using System;

namespace Notes.Models
{
    public class Note : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created_at { get; set; }
    }
}