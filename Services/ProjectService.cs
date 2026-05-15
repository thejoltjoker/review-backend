using AutoMapper;
using Review.Api.Models;
using Review.Api.Models.DTOs;
using Review.Api.Repositories;

namespace Review.Api.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProjectAuthorizationService _projectAuthorizationService;
    private readonly IMapper _mapper;


    public ProjectService(
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            IProjectAuthorizationService projectAuthorizationService,
            IMapper mapper)
        // TODO Improve error handling
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _projectAuthorizationService = projectAuthorizationService;
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
        User? user = await _userRepository.GetByIdAsync(userId);
        // TODO Return EntityStatus instead of throwing
        if (user == null) throw new KeyNotFoundException("User not found");


        Project project = new(data.Name, userId);

        project.ProjectUsers.Add(new ProjectUser
        {
            ProjectId = project.Id,
            UserId = user.Id,
            Project = project,
            User = user,
            Role = ProjectUserRole.Owner
        });

        Project result = await _projectRepository.AddAsync(project);
        await _projectRepository.SaveAsync();
        return _mapper.Map<ProjectDto>(result);
    }

    public async Task<EntityStatus> UpdateAsync(string userId, string projectId, UpdateProjectDto data)
    {
        var existing = await _projectRepository.GetByIdForUserAsync(userId, projectId);
        if (existing == null) return EntityStatus.NotFound;

        var canUpdate = await _projectAuthorizationService.CanAsync(userId, projectId, ProjectPermission.Update);
        if (!canUpdate) return EntityStatus.Forbidden;

        existing.Name = data.Name;
        _projectRepository.Update(existing);
        await _projectRepository.SaveAsync();
        return EntityStatus.Updated;
    }

    public async Task<EntityStatus> DeleteAsync(string userId, string projectId)
    {
        var project = await _projectRepository.GetByIdForUserAsync(userId, projectId);
        if (project == null) return EntityStatus.NotFound;

        var canDelete = await _projectAuthorizationService.CanAsync(userId, projectId, ProjectPermission.Delete);
        if (!canDelete) return EntityStatus.Forbidden;

        _projectRepository.Delete(project);
        await _projectRepository.SaveAsync();
        return EntityStatus.Deleted;
    }
}