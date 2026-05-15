using Review.Api.Models;
using Review.Api.Models.DTOs;

namespace Review.Api.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync(string userId);
    Task<ProjectWithAssetsDto?> GetByIdAsync(string userId, string projectId);
    Task<ProjectDto> CreateAsync(string userId, CreateProjectDto data);
    Task<EntityStatus> UpdateAsync(string userId, string projectId, UpdateProjectDto data);
    Task<EntityStatus> DeleteAsync(string userId, string projectId);
}