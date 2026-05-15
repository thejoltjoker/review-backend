using Review.Api.Models;
using Review.Api.Repositories;

namespace Review.Api.Services;

public class ProjectAuthorizationService : IProjectAuthorizationService
{
    private readonly IProjectRepository _projectRepository;

    private static readonly Dictionary<ProjectUserRole, HashSet<ProjectPermission>> RolePermissions = new()
    {
        [ProjectUserRole.Viewer] = [ProjectPermission.Read],
        [ProjectUserRole.Editor] = [ProjectPermission.Read, ProjectPermission.Update],
        [ProjectUserRole.Owner] = [ProjectPermission.Read, ProjectPermission.Update, ProjectPermission.Delete]
    };

    public ProjectAuthorizationService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<bool> CanAsync(string userId, string projectId, ProjectPermission permission)
    {
        var project = await _projectRepository.GetByIdForUserAsync(userId, projectId);
        if (project == null) return false;

        var projectUser = project.ProjectUsers.FirstOrDefault(user => user.UserId == userId);
        if (projectUser == null) return false;

        return RolePermissions[projectUser.Role].Contains(permission);
    }
}