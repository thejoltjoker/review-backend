using AutoMapper;
using Review.Api.Models;
using Review.Api.Models.DTOs;
using Review.Api.Repositories;

namespace Review.Api.Services;

public class ApiKeyService : IApiKeyService
{
    private readonly IApiKeyRepository _apiKeyRepository;
    private readonly IMapper _mapper;

    public ApiKeyService(IApiKeyRepository apiKeyRepository, IMapper mapper)
    {
        _apiKeyRepository = apiKeyRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ApiKeyDto>> GetAllAsync(string userId)
    {
        var result = await _apiKeyRepository.GetAllByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<ApiKeyDto>>(result);
    }

    public async Task<ApiKeyDto> CreateAsync(string userId, string? name)
    {
        // TODO Invalidate old api key when new is created
        var newKey = new ApiKey(userId, name);
        var result = await _apiKeyRepository.AddAsync(newKey);
        await _apiKeyRepository.SaveAsync();
        return _mapper.Map<ApiKeyDto>(result);
    }
}