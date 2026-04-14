using Review.Api.Models;

namespace Review.Api.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id);
    Task SaveAsync();
}