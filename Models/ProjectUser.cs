using System.ComponentModel.DataAnnotations;

namespace Review.Api.Models;

public class ProjectUser
{
    [Required]
    [StringLength(255)]
    public string ProjectId { get; set; } = string.Empty;
    [Required]
    [StringLength(255)]
    public string UserId { get; set; } = string.Empty;
    [Required]
    public ProjectUserRole Role { get; set; } = ProjectUserRole.Viewer;
    public Project Project { get; set; } = null!;
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}