namespace Review.Api.Models;

public class Asset
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public string FileType { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? ProjectId { get; set; }
    public Project? Project { get; set; }

    public string? UserId { get; set; }
    public User? User { get; set; }
    
    public ICollection<Comment> Comments { get; } = new List<Comment>(); // Collection navigation containing dependents
}