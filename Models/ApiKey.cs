namespace Review.Api.Models;

public class ApiKey
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? Name { get; set; }
    public string KeyHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public string UserId { get; set; }
    public User User { get; set; } = null!;
    // TODO implement createdAt and updatedAt
}