using Review.Api.Models;
using Review.Api.Models.DTOs;

namespace Review.Api.Services;

public interface IApiKeyService
{
    Task<IEnumerable<ApiKeyDto>> GetAllAsync(string userId);
    Task<ApiKeyDto> CreateAsync(string userId, string? name);
}