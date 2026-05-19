using Review.Api.Models;
using Review.Api.Models.DTOs;

namespace Review.Api.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync(string userId);
    Task<ProjectWithAssetsDto?> GetByIdAsync(string userId, string projectId);
    Task<(EntityStatus Status, ProjectDto? Project)> CreateAsync(string userId, CreateProjectDto data);
    Task<bool> UpdateAsync(string userId, string projectId, UpdateProjectDto data);
    Task<bool> DeleteAsync(string userId, string projectId);
}