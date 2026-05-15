using Review.Api.Models;

namespace Review.Api.Services;

public interface IProjectAuthorizationService
{
    public bool CanReadProjectAsync(string userId, string projectId);
    public bool CanUpdateProject(string userId, string projectId);
    public bool CanDeleteProject(string userId, string projectId);
    public Task<bool> Can(string userId, string projectId, ProjectPermission action);
}