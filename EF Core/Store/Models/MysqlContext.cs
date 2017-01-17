using Microsoft.EntityFrameworkCore;

namespace Store.Models
{
    public class MysqlContext : DbContext
    {
        public MysqlContext(DbContextOptions<MysqlContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}