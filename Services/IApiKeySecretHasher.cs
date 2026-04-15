using Microsoft.AspNetCore.Identity;

namespace Review.Api.Services;

public interface IApiKeySecretHasher
{
    string Hash(string secret);
    PasswordVerificationResult Verify(string hashedSecret, string providedSecret);
}