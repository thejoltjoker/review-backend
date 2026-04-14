using System.ComponentModel.DataAnnotations;

namespace Review.Api.Models.DTOs;

public class CreateProjectDto
{
    [Required]
    [StringLength(128, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}

public class ProjectDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class ProjectWithAssetsDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<AssetDto> Assets { get; set; } = [];
}