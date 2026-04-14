using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Review.Api.Models;

namespace Review.Api.Contexts;

public class IdentityContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Ignore<ProjectUser>();
        builder.Ignore<Project>();
        builder.Ignore<Asset>();
        builder.Ignore<Comment>();
        builder.Ignore<ApiKey>();
    }
};