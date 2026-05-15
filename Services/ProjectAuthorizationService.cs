using Review.Api.Models;
using Review.Api.Repositories;

namespace Review.Api.Services;

public class ProjectAuthorizationService : IProjectAuthorizationService
{
    private readonly IProjectRepository _projectRepository;

    public List<ProjectPermission> GetPermissionFromRole(ProjectUserRole role)
    {
        var permissions = new Dictionary<ProjectUserRole, List<ProjectPermission>>
        {
            [ProjectUserRole.Viewer] = [ProjectPermission.Read],
            [ProjectUserRole.Editor] = [ProjectPermission.Read, ProjectPermission.Update],
            [ProjectUserRole.Owner] = [ProjectPermission.Read, ProjectPermission.Update, ProjectPermission.Delete]
        };
        return permissions[role];
    }

    public ProjectAuthorizationService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<bool> CanReadProjectAsync(string userId, string projectId)
    {
        var project = await _projectRepository.GetByIdForUserAsync(userId, projectId);
        if (project == null) return false;
        var projectUser = project.ProjectUsers.FirstOrDefault(pu => pu.UserId == userId);
        if (projectUser == null) return false;
        if (GetPermissionFromRole(projectUser.Role).Contains(ProjectPermission.Read)) return true;
        return false;
    }

    public bool CanUpdateProject(string userId, string projectId)
    {
        throw new NotImplementedException();
    }

    public bool CanDeleteProject(string userId, string projectId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Can(string userId, string projectId, ProjectPermission action)
    {
        var project = await _projectRepository.GetByIdForUserAsync(userId, projectId);
        if (project == null) return false;

        var projectUser = project.ProjectUsers.FirstOrDefault(u => u.UserId == userId);

        var permissions = new Dictionary<ProjectUserRole, List<ProjectPermission>>
        {
            [ProjectUserRole.Viewer] = [ProjectPermission.Read],
            [ProjectUserRole.Editor] = [ProjectPermission.Read, ProjectPermission.Update],
            [ProjectUserRole.Owner] = [ProjectPermission.Read, ProjectPermission.Update, ProjectPermission.Delete]
        };
        return permissions[projectUser!.Role].Contains(action);
    }
}