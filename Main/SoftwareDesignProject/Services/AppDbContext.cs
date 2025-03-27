using Microsoft.EntityFrameworkCore;
using SoftwareDesignProject.Models.Entities;

namespace SoftwareDesignProject.Services;

public class AppDbContext: DbContext
{
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        modelBuilder.Entity<Plugin>()
            .HasIndex(u => u.Name)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasMany(u => u.Starred)
            .WithMany()
            .UsingEntity(j => j.ToTable("user_starred_plugins"));
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Host=localhost;Database=ittools;Username=postgres;Password=1234");
    }
}