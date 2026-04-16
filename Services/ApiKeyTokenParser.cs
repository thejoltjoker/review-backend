namespace Review.Api.Services;

public class ApiKeyTokenParser
{
    public static bool ParseToken(string token, out string keyId, out string secret)
    {
        keyId = string.Empty;
        secret = string.Empty;

        if (string.IsNullOrEmpty(token)) return false;

        var parts = token.Split('.', 2);

        if (parts.Length != 2) return false;
        if (!parts[0].StartsWith("ak_", StringComparison.Ordinal)) return false;
        if (parts[0].Length <= 3) return false;
        if (string.IsNullOrEmpty(parts[1])) return false;

        keyId = parts[0];
        secret = parts[1];
        return true;
    }

    public static string CreateToken(string keyId, string secret)
    {
        return $"{keyId}.{secret}";
    }
}