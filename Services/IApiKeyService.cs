using Review.Api.Models;
using Review.Api.Models.DTOs;

namespace Review.Api.Services;

public interface IApiKeyService
{
    Task<ApiKeyDto> CreateAsync(string userId);
}