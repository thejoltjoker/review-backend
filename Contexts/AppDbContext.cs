using Microsoft.EntityFrameworkCore;
using Review.Api.Models;

namespace Review.Api.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User relations
        // modelBuilder.Entity<User>()
        //     .HasOne(e => e.ApiKey)
        //     .WithOne(e => e.User)
        //     .HasForeignKey<ApiKey>(e => e.UserId)
        //     .IsRequired();
        // modelBuilder.Entity<User>()
        //     .HasMany(e => e.Projects)
        //     .WithMany(e => e.Users);
        // modelBuilder.Entity<User>()
        //     .HasMany(e => e.Assets)
        //     .WithOne(e => e.User)
        //     .HasForeignKey(e => e.UserId)
        //     .IsRequired(false);
        // modelBuilder.Entity<User>()
        //     .HasMany(e => e.Comments)
        //     .WithOne(e => e.User)
        //     .HasForeignKey(e => e.UserId)
        //     .IsRequired(false);

        // Project relations
        // modelBuilder.Entity<Project>()
        //     .HasMany(e => e.Assets)
        //     .WithOne(e => e.Project)
        //     .HasForeignKey(e => e.ProjectId)
        //     .IsRequired(false);
    }
}