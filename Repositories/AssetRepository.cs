using Microsoft.EntityFrameworkCore;
using Review.Api.Contexts;
using Review.Api.Models;

namespace Review.Api.Repositories;

public class AssetRepository(ApplicationDbContext context) : IAssetRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Asset>> GetAllByUserIdAsync(string userId) =>
        await _context.Assets.AsNoTracking()
            .Where(asset => asset.UserId == userId)
            .ToListAsync();


    public async Task<List<Asset>> GetAllByProjectIdAsync(string userId, string projectId) =>
        await _context.Assets.AsNoTracking()
            .Where(asset => asset.ProjectId == projectId &&
                            asset.Project != null &&
                            asset.Project.Users.Any(p => p.Id == userId))
            .ToListAsync();


    public async Task<Asset?> GetByIdAsync(string userId, string assetId) =>
        await _context.Assets.AsNoTracking()
            .FirstOrDefaultAsync(asset => asset.UserId == userId && asset.Id == assetId);


    public async Task<Asset> AddAsync(Asset asset)
    {
        await _context.Assets.AddAsync(asset);
        return asset;
    }

    public void Update(Asset asset) => _context.Assets.Update(asset);


    public void Delete(Asset asset) => _context.Assets.Remove(asset);


    public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
}