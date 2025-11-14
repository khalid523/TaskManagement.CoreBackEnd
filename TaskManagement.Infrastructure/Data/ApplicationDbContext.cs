using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.Entities;

namespace TaskManagement.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<WorkTask> WorkTasks { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired();
        modelBuilder.Entity<User>()
            .HasMany(u => u.Tasks)
            .WithOne(t => t.AssignedToUser)
            .HasForeignKey(t => t.AssignedToUserId);

        // WorkTask configuration
        modelBuilder.Entity<WorkTask>()
            .HasKey(t => t.Id);
        modelBuilder.Entity<WorkTask>()
            .Property(t => t.Title)
            .IsRequired();

        // Seed data
        var adminUser = new User
        {
            Id = 1,
            Name = "Admin User",
            Email = "admin@example.com",
            Role = "Admin",
            CreatedAt = DateTime.UtcNow
        };

        var regularUser = new User
        {
            Id = 2,
            Name = "John Doe",
            Email = "john@example.com",
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        modelBuilder.Entity<User>().HasData(adminUser, regularUser);

        modelBuilder.Entity<WorkTask>().HasData(
            new WorkTask
            {
                Id = 1,
                Title = "Setup Database",
                Description = "Initialize database schema",
                Status = "Completed",
                AssignedToUserId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new WorkTask
            {
                Id = 2,
                Title = "Build API",
                Description = "Implement REST API endpoints",
                Status = "InProgress",
                AssignedToUserId = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new WorkTask
            {
                Id = 3,
                Title = "Create Frontend",
                Description = "Build Angular application",
                Status = "Pending",
                AssignedToUserId = 2,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
    }
}