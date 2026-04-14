using Review.Api.Models;
using Review.Api.Models.DTOs;

namespace Review.Api.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync();
    Task<ProjectWithAssetsDto?> GetByIdAsync(string id);
    Task<ProjectDto> CreateAsync(string userId, CreateProjectDto data);
    Task<bool> UpdateAsync(string id, Project project);
    Task<bool> DeleteAsync(string id);
}