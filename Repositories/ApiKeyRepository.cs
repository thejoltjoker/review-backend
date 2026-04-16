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
        ApiKeyTokenParser.ParseToken(token, out var keyId, out var secret);
        if (string.IsNullOrEmpty(keyId)) return null;

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

    public void Revoke(ApiKey apiKey)
    {
        apiKey.RevokedAt = DateTime.UtcNow;
    }

    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }
}