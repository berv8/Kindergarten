using Microsoft.EntityFrameworkCore;

namespace Kindergarten.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
       : base(options)
        {
        }
        public DbSet<Login> Logins { get; set; }

    }
}
