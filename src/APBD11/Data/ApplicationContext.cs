using APBD11.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD11.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Role> Role { get; set; }
    public DbSet<Account> Account { get; set; }
    public DbSet<Device> Device { get; set; }
    
    public DbSet<DeviceType> DeviceType { get; set; }
    public DbSet<Position> Position { get; set; }
    
    public DbSet<Employee> Employee { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = Models.Role.Admin },
            new Role { Id = 2, Name = Models.Role.User }
        );

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Username)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}