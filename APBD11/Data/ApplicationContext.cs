using APBD11.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD11.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Device> Devices { get; set; }
    
    public DbSet<DeviceType> DeviceTypes { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = Role.Admin },
            new Role { Id = 2, Name = Role.User }
        );

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Username)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}