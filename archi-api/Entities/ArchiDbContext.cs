using Microsoft.EntityFrameworkCore;

namespace Archi.Entities;

public class ArchiDbContext : DbContext
{
    public ArchiDbContext(DbContextOptions<ArchiDbContext> options)
    : base(options)
    {
    }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
}
