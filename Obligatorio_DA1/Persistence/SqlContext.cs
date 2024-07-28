using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class SqlContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Promotion> Promotions { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<Deposit> Deposits { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<Availability> Availabilities { get; set; }

    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
        if (!Database.IsInMemory())
        {
            this.Database.Migrate();
        }
    }
}