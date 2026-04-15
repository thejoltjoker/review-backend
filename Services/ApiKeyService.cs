using AutoMapper;
using Review.Api.Models;
using Review.Api.Models.DTOs;
using Review.Api.Repositories;

namespace Review.Api.Services;

public class ApiKeyService : IApiKeyService
{
    private readonly IApiKeyRepository _apiKeyRepository;
    private readonly IMapper _mapper;
    private readonly IApiKeySecretHasher _hasher;

    public ApiKeyService(IApiKeyRepository apiKeyRepository, IMapper mapper, IApiKeySecretHasher hasher)
    {
        _apiKeyRepository = apiKeyRepository;
        _mapper = mapper;
        _hasher = hasher;
    }

    public async Task<IEnumerable<ApiKeyDto>> GetAllAsync(string userId)
    {
        var result = await _apiKeyRepository.GetAllByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<ApiKeyDto>>(result);
    }

    public async Task<ApiKeyCreatedDto> CreateAsync(string userId, string? name)
    {
        // TODO Invalidate old api key when new is created
        string keyId = ApiKeyGenerator.GenerateApiKeyId();
        string secret = ApiKeyGenerator.GenerateApiKey();
        string secretHash = _hasher.Hash(secret);
        string token = $"{keyId}.{secret}";

        ApiKey newKey = new ApiKey(userId: userId, keyHash: secretHash, keyId: keyId, name: name);
        await _apiKeyRepository.AddAsync(newKey);
        await _apiKeyRepository.SaveAsync();
        ApiKeyCreatedDto response = new(token, name);
        return response;
    }
}