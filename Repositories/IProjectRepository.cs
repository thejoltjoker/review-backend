using Review.Api.Models;

namespace Review.Api.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync();
    Task<List<Project>> GetAllByUserIdAsync(string userId);
    Task<Project?> GetByIdAsync(string projectId);
    Task<Project?> GetByIdForUserAsync(string userId, string projectId);
    Task<Project> AddAsync(Project project);
    void Update(Project project);
    void Delete(Project project);
    Task SaveAsync();
}