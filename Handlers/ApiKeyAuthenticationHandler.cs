using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Review.Api.Models;
using Review.Api.Repositories;
using Review.Api.Services;

namespace Review.Api.Handlers;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly IApiKeyRepository _repository;
    private readonly IApiKeySecretHasher _hasher;

    public ApiKeyAuthenticationHandler(
        IApiKeyRepository repository,
        IApiKeySecretHasher hasher,
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder
    ) : base(options, logger, encoder)
    {
        _repository = repository;
        _hasher = hasher;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var raw))
            return await Task.FromResult(AuthenticateResult.NoResult());

        if (!AuthenticationHeaderValue.TryParse(raw, out var header))
            return await Task.FromResult(AuthenticateResult.NoResult());

        if (!string.Equals(header.Scheme, ApiKeyConstants.ApiKeyAuthorizationScheme,
                StringComparison.OrdinalIgnoreCase))
            return await Task.FromResult(AuthenticateResult.NoResult());

        var token = header.Parameter;
        if (string.IsNullOrWhiteSpace(token))
            return await Task.FromResult(AuthenticateResult.Fail("Missing API token."));

        // TODO Fix token parsing
        string[] parts = token.Split(".");
        
        if (parts.Length != 2)
            return await Task.FromResult(AuthenticateResult.Fail("Invalid API token."));
        
        if (!parts[0].StartsWith("ak_"))
            return await Task.FromResult(AuthenticateResult.Fail("Invalid API token."));
        
        string keyId = parts[0];
        string secret = parts[1];

        var result = await _repository.GetByKeyId(keyId);
        if (result == null) return await Task.FromResult(AuthenticateResult.Fail("Invalid API token"));
        var verificationResult = _hasher.Verify(result.KeyHash, secret);
        if (verificationResult == PasswordVerificationResult.Failed)
            return await Task.FromResult(AuthenticateResult.Fail("Invalid API token"));

        if (result.ExpiresAt.HasValue && result.ExpiresAt.Value < DateTime.UtcNow)
            return await Task.FromResult(AuthenticateResult.Fail("API key expired"));

        if (result.RevokedAt.HasValue)
            return await Task.FromResult(AuthenticateResult.Fail("API key revoked"));


        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, keyId),
            new Claim(ClaimTypes.NameIdentifier, result.UserId)
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, ApiKeyConstants.ApiKeyName);
        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }
}