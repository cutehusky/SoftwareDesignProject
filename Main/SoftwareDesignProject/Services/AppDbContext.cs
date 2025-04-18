using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SoftwareDesignProject.Models.Entities;

namespace SoftwareDesignProject.Services;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Plugin> Plugins { get; set; }
    public DbSet<User_Plugin> UserPlugins { get; set; }
    public DbSet<Config> Configs { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        modelBuilder.Entity<Plugin>()
            .HasIndex(u => u.Name)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserRoleRef)
            .WithMany()
            .HasForeignKey(u => u.UserRole)
            .OnDelete(DeleteBehavior.Restrict);
        var userRoles = Enum.GetValues(typeof(UserRoles))
            .Cast<UserRoles>()
            .Select(role => new UserRole { Role = role })
            .ToArray();
        modelBuilder.Entity<UserRole>().HasData(userRoles);
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.AddInterceptors(new TimestampInterceptor());
        options.UseNpgsql("Host=localhost;Database=ittools;Username=postgres;Password=1234");
    }
}

public class TimestampInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetTimestamps(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SetTimestamps(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void SetTimestamps(DbContext? context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e is { Entity: ITimestampedEntity, State: EntityState.Added or EntityState.Modified });

        foreach (var entry in entries)
        {
            var entity = (ITimestampedEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}