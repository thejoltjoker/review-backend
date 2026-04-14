using AutoMapper;
using Review.Api.Models;
using Review.Api.Models.DTOs;
using Review.Api.Repositories;

namespace Review.Api.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    private readonly IMapper _mapper;


    public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        // TODO Scope results to the authenticated API-key owner instead of returning all projects.
        var result = await _projectRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProjectDto>>(result);
    }

    public async Task<ProjectWithAssetsDto?> GetByIdAsync(string id)
    {
        // TODO Verify project ownership for the authenticated API-key owner before returning protected data.
        var result = await _projectRepository.GetByIdAsync(id);
        if (result == null) return null;
        return _mapper.Map<ProjectWithAssetsDto>(result);
    }

    public async Task<ProjectDto> CreateAsync(string userId, CreateProjectDto data)
    {
        // TODO validate data
        var project = _mapper.Map<Project>(data);
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("User not found");
        if (project.Users.All(u => u.Id != user.Id))
        {
            project.Users.Add(user);
        }

        var result = await _projectRepository.AddAsync(project);
        await _projectRepository.SaveAsync();
        return _mapper.Map<ProjectDto>(result);
    }

    public async Task<bool> UpdateAsync(string id, Project project)
    {
        // TODO validate data
        // TODO Enforce that only the owner tied to the validated API key can update this project.
        var existing = await _projectRepository.GetByIdAsync(id);
        if (existing == null) return false;
        existing.Name = project.Name;
        existing.CreatedAt = project.CreatedAt;
        _projectRepository.Update(existing);
        await _projectRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        // TODO Enforce that only the owner tied to the validated API key can delete this project.
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null) return false;
        _projectRepository.Delete(project);
        await _projectRepository.SaveAsync();
        return true;
    }
}