namespace Review.Api.Models;

public class Comment : BaseEntity
{
    public Comment(string content, float timestampSeconds, string assetId, string userId)
    {
        Content = content;
        TimestampSeconds = timestampSeconds;
        AssetId = assetId;
        UserId = userId;
    }

    public string Content { get; set; }
    public float TimestampSeconds { get; set; }

    public string AssetId { get; set; }
    public Asset? Asset { get; set; }

    public string UserId { get; set; }
    public User? User { get; set; }
}