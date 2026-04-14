using Review.Api.Models;
using Review.Api.Repositories;

namespace Review.Api.Services;

public class ProjectService(IProjectRepository repository) : IProjectService
{
    private readonly IProjectRepository _repository = repository;

    public async Task<List<Project>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Project?> GetByIdAsync(string id) => await _repository.GetByIdAsync(id);

    public async Task<Project> CreateAsync(Project project)
    {
        // TODO validate data
        await _repository.AddAsync(project);
        await _repository.SaveAsync();
        return project;
    }

    public async Task<bool> UpdateAsync(string id, Project project)
    {
        // TODO validate data
        var existing = await GetByIdAsync(id);
        if (existing == null) return false;
        existing.Name = project.Name;
        existing.CreatedAt = project.CreatedAt;
        _repository.Update(existing);
        await _repository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var project = await GetByIdAsync(id);
        if (project == null) return false;
        _repository.Delete(project);
        await _repository.SaveAsync();
        return true;
    }
}