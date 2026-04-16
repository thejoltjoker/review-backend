using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Review.Api.Contexts;
using Review.Api.Models;
using Review.Api.Models.DTOs;
using Review.Api.Services;

namespace Review.Api.Repositories;

public class ApiKeyRepository : IApiKeyRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IApiKeySecretHasher _hasher;

    public ApiKeyRepository(ApplicationDbContext context, IApiKeySecretHasher hasher)
    {
        _context = context;
        _hasher = hasher;
    }

    public async Task<List<ApiKey>> GetAllAsync()
    {
        return await _context.ApiKeys.ToListAsync();
    }

    public async Task<List<ApiKey>> GetAllByUserIdAsync(string userId)
    {
        return await _context.ApiKeys.Where(apiKey => apiKey.UserId == userId).ToListAsync();
    }

    public async Task<ApiKey?> GetByTokenAsync(string token)
    {
        // TODO Improve token parsing
        string[] parts = token.Split(".");
        if (parts.Length != 2) return null;
        if (!parts[0].StartsWith("ak_")) return null;
        string keyId = parts[0];

        return await _context.ApiKeys
            .Include(apiKey => apiKey.User)
            .Where(apiKey => apiKey.KeyId == keyId)
            .FirstOrDefaultAsync();
    }

    public async Task<ApiKey?> GetByKeyId(string keyId)
    {
        return await _context.ApiKeys
            .Include(apiKey => apiKey.User)
            .Where(apiKey => apiKey.KeyId == keyId)
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