namespace Review.Api.Models.DTOs;

public class AssetDto
{
    public string Id { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    // public string? UserId { get; set; }
    // public User? User { get; set; }
    
    // public ICollection<Comment> Comments { get; } = new List<Comment>(); // Collection navigation containing dependents
}