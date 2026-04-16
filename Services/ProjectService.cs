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
    // TODO Improve error handling
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync(string userId)
    {
        var result = await _projectRepository.GetAllByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<ProjectDto>>(result);
    }

    public async Task<ProjectWithAssetsDto?> GetByIdAsync(string userId, string projectId)
    {
        var result = await _projectRepository.GetByIdForUserAsync(userId, projectId);
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

    public async Task<bool> UpdateAsync(string userId, string projectId, UpdateProjectDto data)
    {
        var existing = await _projectRepository.GetByIdForUserAsync(userId, projectId);
        if (existing == null) return false;
        existing.Name = data.Name;
        _projectRepository.Update(existing);
        await _projectRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string userId, string projectId)
    {
        // TODO Enforce that only the owner tied to the validated API key can delete this project.
        var project = await _projectRepository.GetByIdForUserAsync(userId, projectId);
        if (project == null) return false;
        _projectRepository.Delete(project);
        await _projectRepository.SaveAsync();
        return true;
    }
}