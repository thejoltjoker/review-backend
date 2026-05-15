using System.Collections.Generic;

namespace Review.Api.Models;

public class Asset : BaseEntity
{
    
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;

    public string ProjectId { get; set; } = string.Empty;
    public Project Project { get; set; }

    public string UserId { get; set; } = string.Empty;
    public User User { get; set; }
    
    public ICollection<Comment> Comments { get; } = new List<Comment>(); // Collection navigation containing dependents
}