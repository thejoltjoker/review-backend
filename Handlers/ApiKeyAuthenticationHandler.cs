using System.Security.Claims;
using System.Security.Principal;
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
        if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        
        string authHeader = Request.Headers[HeaderNames.Authorization];
        string apiKey = authHeader.Split(" ")[1];
        
        if (_keys.Contains(apiKey))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, apiKey)
              // TODO add user info  
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Api Key");
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        
        return Task.FromResult(AuthenticateResult.Fail("Not implemented"));
    }
}