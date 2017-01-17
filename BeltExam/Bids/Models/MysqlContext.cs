using Microsoft.EntityFrameworkCore;

namespace BeltExam.Models
{
    public class MysqlContext : DbContext
    {
        public MysqlContext(DbContextOptions<MysqlContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
    }
}