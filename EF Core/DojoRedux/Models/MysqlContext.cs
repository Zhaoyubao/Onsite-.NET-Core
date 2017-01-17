using Microsoft.EntityFrameworkCore;

namespace QuotingRedux.Models
{
    public class MysqlContext : DbContext
    {
        public MysqlContext(DbContextOptions<MysqlContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Quote> Quotes { get; set; }
    }
}