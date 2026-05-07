using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Review.Api.Models;

namespace Review.Api.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ProjectUser>(entity =>
        {
            entity.HasKey(projectUser => new { projectUser.ProjectId, projectUser.UserId });

            entity.Property(projectUser => projectUser.ProjectId).IsRequired().HasMaxLength(255);
            entity.Property(projectUser => projectUser.UserId).IsRequired().HasMaxLength(255);
            entity.Property(projectUser => projectUser.Role)
                .IsRequired()
                .HasConversion<int>();

            entity.HasOne(projectUser => projectUser.Project)
                .WithMany(project => project.ProjectUsers)
                .HasForeignKey(projectUser => projectUser.ProjectId);
            entity.HasOne(projectUser => projectUser.User)
                .WithMany(user => user.ProjectUsers)
                .HasForeignKey(projectUser => projectUser.UserId);
        });
    }

    private void SetTimestamps()
    {
        DateTime now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity.CreatedAt == default)
                {
                    entry.Entity.CreatedAt = now;
                }

                entry.Entity.UpdatedAt = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }
    }

    public override int SaveChanges()
    {
        SetTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }
}