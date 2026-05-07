using System;
using System.ComponentModel.DataAnnotations;

namespace Review.Api.Models.DTOs;

public class CommentDto
{
    public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public float TimestampSeconds { get; set; }
    public string AssetId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class CreateCommentDto
{
    [Required] [StringLength(512)] public string Content { get; set; }
    [Required] [Range(0, float.MaxValue)] public float TimestampSeconds { get; set; }
    [Required] public string AssetId { get; set; }
    [Required] public string UserId { get; set; }
}

public class UpdateCommentDto
{
    [Required] [StringLength(512)] public string Content { get; set; }
    [Required] [Range(0, float.MaxValue)] public float TimestampSeconds { get; set; }
}