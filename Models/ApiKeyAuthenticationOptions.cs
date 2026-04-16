using Microsoft.AspNetCore.Authentication;

namespace Review.Api.Models;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string DisplayMessage { get; set; }
}