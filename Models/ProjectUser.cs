namespace Review.Api.Models;

public class ProjectUser(ProjectUserRole role = ProjectUserRole.Viewer)
{
    public string ProjectId { get; set; }
    public string UserId { get; set; }
    
    public ProjectUserRole Role { get; set; } = role;
    
    public Project Project { get; set; }
    public User User { get; set; }
}