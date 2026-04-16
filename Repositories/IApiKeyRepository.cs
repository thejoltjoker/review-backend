using Microsoft.EntityFrameworkCore.ChangeTracking;
using Review.Api.Models;

namespace Review.Api.Repositories;

public interface IApiKeyRepository
{
    Task<List<ApiKey>> GetAllAsync();
    Task<List<ApiKey>> GetAllByUserIdAsync(string userId);
    Task<ApiKey?> GetByTokenAsync(string token);
    Task<ApiKey?> GetByKeyId(string keyId);
    Task<ApiKey> AddAsync(ApiKey apiKey);
    void Revoke(ApiKey apiKey);
    Task SaveAsync();
}