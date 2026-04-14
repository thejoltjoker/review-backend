using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Review.Api.Models;

namespace Review.Api.Contexts;

public class IdentityContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
};