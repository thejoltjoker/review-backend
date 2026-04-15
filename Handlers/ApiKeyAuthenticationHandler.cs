using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Review.Api.Models;
using Review.Api.Repositories;

namespace Review.Api.Handlers;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private readonly IApiKeyRepository _repository;

    public ApiKeyAuthenticationHandler(
        IApiKeyRepository repository,
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder
    ) : base(options, logger, encoder)
    {
        _repository = repository;
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

        var apiKey = header.Parameter;
        if (string.IsNullOrWhiteSpace(apiKey))
            return await Task.FromResult(AuthenticateResult.Fail("Missing API key."));

        var result = await _repository.GetByValueAsync(apiKey);
        if (result == null) return await Task.FromResult(AuthenticateResult.Fail("Invalid API key"));

        if (result.ExpiresAt.HasValue && result.ExpiresAt.Value < DateTime.UtcNow)
            return await Task.FromResult(AuthenticateResult.Fail("API key expired"));

        if (result.RevokedAt.HasValue)
            return await Task.FromResult(AuthenticateResult.Fail("API key revoked"));
    


        var claims = new List<Claim>
        {
            // TODO Do not expose the API key as a claim, change to a non-secret identifier i.e. id/name.
            new Claim(ClaimTypes.Name, apiKey),
            new Claim(ClaimTypes.NameIdentifier, result.UserId)
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, ApiKeyConstants.ApiKeyName);
        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }
}