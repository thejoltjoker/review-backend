using Microsoft.AspNetCore.Identity;

namespace Review.Api.Models;

public class User : IdentityUser
{
    public ApiKey? ApiKey { get; set; } 

}