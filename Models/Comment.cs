namespace Review.Api.Models;

public class Comment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Content { get; set; }
    public float TimestampSeconds { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? AssetId { get; set; }
    public Asset? Asset { get; set; }

    public string? UserId { get; set; }
    public User? User { get; set; }
}