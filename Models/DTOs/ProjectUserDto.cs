namespace Review.Api.Models.DTOs;

public class ProjectUserDto
{
    public string ProjectId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public ProjectUserRole Role { get; set; } = ProjectUserRole.Viewer;
    public UserDto? User { get; set; }
}