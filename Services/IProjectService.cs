using Review.Api.Models;

namespace Review.Api.Services;

public interface IProjectService
{
    Task<List<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(string id);
    Task<Project> CreateAsync(Project project);
    Task<bool> UpdateAsync(string id, Project project);
    Task<bool> DeleteAsync(string id);
}