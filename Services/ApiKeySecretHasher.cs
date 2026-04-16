using Microsoft.AspNetCore.Identity;
using Review.Api.Models;

namespace Review.Api.Services;

public class ApiKeySecretHasher : IApiKeySecretHasher
{
    private readonly PasswordHasher<ApiKey> _hasher = new();

    public string Hash(string secret)
    {
        return _hasher.HashPassword(user: null!, password: secret);
    }

    public PasswordVerificationResult Verify(string hashedSecret, string providedSecret)
    {
        return _hasher.VerifyHashedPassword(
            user: null!,
            hashedPassword: hashedSecret,
            providedPassword: providedSecret
        );
    }
}