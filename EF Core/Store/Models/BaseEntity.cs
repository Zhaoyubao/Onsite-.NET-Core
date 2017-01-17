using System;

namespace Store.Models
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}