// Source: https://generate-random.org/api-keys/csharp

using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;

namespace Review.Api.Services;

public static class ApiKeyGenerator
{


    public static string GenerateApiKeyId(int length = 12)
    {
        using var generator = RandomNumberGenerator.Create();
        byte[] bytes = new byte[length];
        generator.GetBytes(bytes);

        var encoded = WebEncoders.Base64UrlEncode(bytes);
        return $"ak_{encoded}";
    }

    public static string GenerateApiKey(int length = 32)
    {
        using var generator = RandomNumberGenerator.Create();
        byte[] bytes = new byte[length];
        generator.GetBytes(bytes);
        
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }

    public static string GenerateApiKeyBase64(int length = 32)
    {
        using var generator = RandomNumberGenerator.Create();
        byte[] bytes = new byte[length];
        generator.GetBytes(bytes);
        
        return Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}