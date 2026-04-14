using Microsoft.EntityFrameworkCore;
using Review.Api.Contexts;
using Review.Api.Models;

namespace Review.Api.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;


    public async Task<User?> GetByIdAsync(string id) =>
        await _context.Users.FirstOrDefaultAsync(p => p.Id == id);

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}