using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Review.Api.Models;
using Review.Api.Models.DTOs;
using Review.Api.Repositories;

namespace Review.Api.Services;

public class AssetService : IAssetService
{
    private readonly IAssetRepository _repository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;


    public AssetService(IAssetRepository repository, IProjectRepository projectRepository, IMapper mapper)
    {
        _repository = repository;
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AssetDto>> GetAllAsync(string userId)
    {
        List<Asset> result = await _repository.GetAllByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<AssetDto>>(result);
    }

    public async Task<AssetWithCommentsDto?> GetByIdAsync(string userId, string assetId)
    {
        Asset? result = await _repository.GetByIdAsync(userId, assetId);
        return _mapper.Map<AssetWithCommentsDto>(result);
    }

    public async Task<AssetDto> CreateAsync(string userId, CreateAssetDto data)
    {
        Asset asset = _mapper.Map<Asset>(data);
        // TODO check if user on project
        asset.UserId = userId;
        await _repository.AddAsync(asset);
        await _repository.SaveAsync();
        return _mapper.Map<AssetDto>(asset);
    }

    public async Task<EntityStatus> UpdateAsync(string userId, string assetId, UpdateAssetDto data)
    {
        Asset? asset = await _repository.GetByIdAsync(userId, assetId);
        if (asset == null) return EntityStatus.NotFound;

        bool hasChanges = false;

        if (data.FileName != asset.FileName)
        {
            asset.FileName = data.FileName;
            hasChanges = true;
        }

        if (data.FileUrl != asset.FileUrl)
        {
            asset.FileUrl = data.FileUrl;
            hasChanges = true;
        }

        if (data.FileType != asset.FileType)
        {
            asset.FileType = data.FileType;
            hasChanges = true;
        }

        if (!string.IsNullOrWhiteSpace(data.ProjectId) && data.ProjectId != asset.ProjectId)
        {
            Project? project = await _projectRepository.GetByIdForUserAsync(userId, data.ProjectId);
            if (project == null) return EntityStatus.InvalidReference;
            asset.ProjectId = data.ProjectId;
            hasChanges = true;
        }

        if (!hasChanges) return EntityStatus.NoChanges;

        _repository.Update(asset);
        await _repository.SaveAsync();
        return EntityStatus.Updated;
    }

    public async Task<EntityStatus> DeleteAsync(string userId, string assetId)
    {
        var asset = await _repository.GetByIdAsync(userId, assetId);
        if (asset == null) return EntityStatus.NotFound;
        _repository.Delete(asset);
        await _repository.SaveAsync();
        return EntityStatus.Deleted;
    }
}