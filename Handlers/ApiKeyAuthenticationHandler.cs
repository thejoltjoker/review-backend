using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Review.Api.Models;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private string[] _keys = ["test-api-key"];
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder
    ) : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var raw))
            return Task.FromResult(AuthenticateResult.NoResult());

        if (!AuthenticationHeaderValue.TryParse(raw, out var header))
            return Task.FromResult(AuthenticateResult.NoResult());

        if (!string.Equals(header.Scheme, ApiKeyConstants.ApiKeyAuthorizationScheme, StringComparison.OrdinalIgnoreCase))
            return Task.FromResult(AuthenticateResult.NoResult());

        var apiKey = header.Parameter;
        if (string.IsNullOrWhiteSpace(apiKey))
            return Task.FromResult(AuthenticateResult.Fail("Missing API key."));
        
        if (_keys.Contains(apiKey))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, apiKey)
              // TODO add user info  
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, ApiKeyConstants.ApiKeyName);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        
        return Task.FromResult(AuthenticateResult.Fail("Not implemented"));
    }
}