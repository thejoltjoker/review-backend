using Review.Api.Services;

namespace Review.Api.Models;

public class ApiKey
{
    public ApiKey(string userId, string? name)
    {
        UserId = userId;
        Name = name;
    }

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? Name { get; set; }

    // TODO Hash key instead of storing plain text
    public string Value { get; set; } = ApiKeyGenerator.GenerateApiKey();
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public string UserId { get; set; }
    public User User { get; set; } = null!;
    // TODO implement createdAt and updatedAt
}