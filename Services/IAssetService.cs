using Review.Api.Models.DTOs;

namespace Review.Api.Services;

public interface IAssetService
{
    Task<IEnumerable<AssetDto>> GetAllAsync(string userId);
    Task<AssetDto?> GetByIdAsync(string userId, string assetId);
    Task<AssetDto> CreateAsync(string userId, CreateAssetDto data);
    Task<bool> UpdateAsync(string userId, string assetId, UpdateAssetDto data);
    Task<bool> DeleteAsync(string userId, string assetId);
}