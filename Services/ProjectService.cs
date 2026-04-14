using AutoMapper;
using Review.Api.Models;
using Review.Api.Models.DTOs;
using Review.Api.Repositories;

namespace Review.Api.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        var result = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProjectDto>>(result);
    }

    public async Task<ProjectWithAssetsDto?> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null) return null;
        return _mapper.Map<ProjectWithAssetsDto>(result);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto data)
    {
        // TODO validate data
        var project = _mapper.Map<Project>(data);
        var result = await _repository.AddAsync(project);
        await _repository.SaveAsync();
        return _mapper.Map<ProjectDto>(result);
    }

    public async Task<bool> UpdateAsync(string id, Project project)
    {
        // TODO validate data
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return false;
        existing.Name = project.Name;
        existing.CreatedAt = project.CreatedAt;
        _repository.Update(existing);
        await _repository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project == null) return false;
        _repository.Delete(project);
        await _repository.SaveAsync();
        return true;
    }
}