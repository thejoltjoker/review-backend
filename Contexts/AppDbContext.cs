using Microsoft.EntityFrameworkCore;

namespace Review.Api.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    
}