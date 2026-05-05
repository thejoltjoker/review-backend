using Review.Api.Models;

namespace Review.Api.Repositories;

public interface IAssetRepository
{
    Task<List<Asset>> GetAllByUserIdAsync(string userId);
    Task<List<Asset>> GetAllByProjectIdAsync(string userId, string projectId);
    Task<Asset?> GetByIdAsync(string userId, string assetId);
    Task<Asset> AddAsync(Asset asset);
    void Update(Asset asset);
    void Delete(Asset asset);
    Task<int> SaveAsync();
}