using Review.Api.Models;
using Review.Api.Models.DTOs;

namespace Review.Api.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync(string userId);
    Task<ProjectWithAssetsDto?> GetByIdAsync(string userId, string projectId);
    Task<ProjectDto> CreateAsync(string userId, CreateProjectDto data);
    Task<bool> UpdateAsync(string userId, string projectId, Project project);
    Task<bool> DeleteAsync(string userId, string projectId);
}