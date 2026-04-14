using Microsoft.EntityFrameworkCore;
using Review.Api.Models;

namespace Review.Api.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ApiKey> ApiKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(e => e.ApiKey)
            .WithOne(e => e.User)
            .HasForeignKey<ApiKey>(e => e.UserId)
            .IsRequired();
    }
}