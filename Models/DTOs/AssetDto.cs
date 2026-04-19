using System.ComponentModel.DataAnnotations;

namespace Review.Api.Models.DTOs;

public class AssetDto
{
    public string Id { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public string ProjectId { get; set; }
}

public class CreateAssetDto
{
    [Required]
    [StringLength(255)]
    [RegularExpression(@"^[^\s]+$", ErrorMessage = "File name cannot contain spaces.")]
    public string FileName { get; set; } = string.Empty;

    [Required] [Url] public string FileUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    [RegularExpression(@"^[^\s]+$", ErrorMessage = "File type cannot contain spaces.")]
    public string FileType { get; set; } = string.Empty;

    [Required] [StringLength(255)] public string ProjectId { get; set; } = string.Empty;
}

public class UpdateAssetDto
{
    [StringLength(255)]
    [RegularExpression(@"^[^\s]+$", ErrorMessage = "File name cannot contain spaces.")]
    public string? FileName { get; set; } = null;

    [Url] public string? FileUrl { get; set; } = null;

    [StringLength(255)]
    [RegularExpression(@"^[^\s]+$", ErrorMessage = "File type cannot contain spaces.")]
    public string? FileType { get; set; } = null;
    
    [StringLength(255)]
    public string? ProjectId { get; set; } = null;
}