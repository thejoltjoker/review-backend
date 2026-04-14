using Review.Api.Models;

namespace Review.Api.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(string id);
    Task AddAsync(Project project);
    void Update(Project project);
    void Delete(Project project);
    Task SaveAsync();
}