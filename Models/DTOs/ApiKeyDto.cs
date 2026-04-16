using System.ComponentModel.DataAnnotations;

namespace Review.Api.Models.DTOs;

public class ApiKeyDto
{
    public string? Name { get; set; }
    public string KeyId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public class CreateApiKeyDto
{
    [StringLength(128)]
    public string? Name { get; set; } = null;
}

public class ApiKeyCreatedDto(string token, string? name)
{
    public string Token { get; set; } = token;
    public string? Name { get; set; } = name;
}