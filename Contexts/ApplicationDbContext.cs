using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Review.Api.Models;

namespace Review.Api.Contexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
{
    private static readonly DateTime SeedCreatedAt = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private const string UserId = "USER0-0000-0000-0000-000000000001";
    private const string Asset1Id = "ASSET000-0000-0000-0000-000000000001";
    private const string Asset2Id = "ASSET000-0000-0000-0000-000000000002";
    private const string Comment1Id = "COMMENT0-0000-0000-0000-000000000001";
    private const string Comment2Id = "COMMENT0-0000-0000-0000-000000000002";
    private const string Project1Id = "PROJECT0-0000-0000-0000-000000000001";
    private const string Project2Id = "PROJECT0-0000-0000-0000-000000000002";

    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // User relations
        // builder.Entity<User>()
        //     .HasOne(e => e.ApiKey)
        //     .WithOne(e => e.User)
        //     .HasForeignKey<ApiKey>(e => e.UserId)
        //     .IsRequired();


        // (
        //     r => r.HasOne<Project>().WithMany().HasForeignKey(e => e.UserId),
        //     l => l.HasOne<User>().WithMany().HasForeignKey(e => e.ProjectId));
        // builder.Entity<User>()
        //     .HasMany(e => e.Assets)
        //     .WithOne(e => e.User)
        //     .HasForeignKey(e => e.UserId)
        //     .IsRequired(false);
        // builder.Entity<User>()
        //     .HasMany(e => e.Comments)
        //     .WithOne(e => e.User)
        //     .HasForeignKey(e => e.UserId)
        //     .IsRequired(false);

        // Project relations
        // builder.Entity<Project>()
        //     .HasMany(e => e.Assets)
        //     .WithOne(e => e.Project)
        //     .HasForeignKey(e => e.ProjectId)
        //     .IsRequired(false);

        // Seed data
        // TODO add users to projects
        builder.Entity<Project>().HasData([
            new()
            {
                Name = "The Code Awakens",
                Id = Project1Id,
                CreatedAt = SeedCreatedAt
            },
            new()
            {
                Name = "Ctrl+Alt+Delight",
                Id = Project2Id,
                CreatedAt = SeedCreatedAt
            }
        ]);

        // TODO add users to assets
        builder.Entity<Asset>().HasData([
            new()
            {
                Id = Asset1Id,
                FileName = "kickoff-notes.mp4",
                FileUrl = "https://cdn.review.local/assets/kickoff-notes.mp4",
                FileType = "video/mp4",
                CreatedAt = SeedCreatedAt,
                ProjectId = Project1Id
            },
            new()
            {
                Id = Asset2Id,
                FileName = "retro-summary.pdf",
                FileUrl = "https://cdn.review.local/assets/retro-summary.pdf",
                FileType = "application/pdf",
                CreatedAt = SeedCreatedAt,
                ProjectId = Project2Id
            }
        ]);

        // TODO add users to comments
        builder.Entity<Comment>().HasData([
            new()
            {
                Id = Comment1Id,
                Content = "Great onboarding section, very clear.",
                TimestampSeconds = 18.5f,
                CreatedAt = SeedCreatedAt,
                AssetId = Asset1Id
            },
            new()
            {
                Id = Comment2Id,
                Content = "Please add a short decision summary at the end.",
                TimestampSeconds = 0f,
                CreatedAt = SeedCreatedAt,
                AssetId = Asset2Id
            }
        ]);
    }
}