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

    public async Task<ApiKeyDto> CreateAsync(string userId)
    {
        // TODO allow user to pick a name
        var newKey = new ApiKey(userId, "");
        var result = await _apiKeyRepository.AddAsync(newKey);
        await _apiKeyRepository.SaveAsync();
        return _mapper.Map<ApiKeyDto>(result);
    }
}