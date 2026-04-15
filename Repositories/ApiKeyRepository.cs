using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Review.Api.Contexts;
using Review.Api.Models;
using Review.Api.Models.DTOs;

namespace Review.Api.Repositories;

public class ApiKeyRepository : IApiKeyRepository
{
    private readonly ApplicationDbContext _context;

    public ApiKeyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ApiKey>> GetAllAsync()
    {
        return await _context.ApiKeys.ToListAsync();
    }

    public async Task<List<ApiKey>> GetAllByUserIdAsync(string userId)
    {
        return await _context.ApiKeys.Where(apiKey => apiKey.UserId == userId).ToListAsync();
    }

    public async Task<ApiKey?> GetByValueAsync(string value)
    {
        return await _context.ApiKeys
            .Include(apiKey => apiKey.User)
            .Where(apiKey => apiKey.Value == value)
            .FirstOrDefaultAsync();
    }

    public async Task<ApiKey> AddAsync(ApiKey apiKey)
    {
        await _context.ApiKeys.AddAsync(apiKey);
        return apiKey;
    }

    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }
}