using Review.Api.Models;

namespace Review.Api.Services;

public interface IProjectAuthorizationService
{
    Task<bool> CanAsync(string userId, string projectId, ProjectPermission permission);
}