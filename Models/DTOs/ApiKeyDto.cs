namespace Review.Api.Models.DTOs;

public class ApiKeyDto
{
    public string? Name { get; set; } = null;
    // TODO Do not store plaintext keys, only return key once at creation
    public string Value { get; set; } = string.Empty; 
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string UserId { get; set; }
}

public class CreateApiKeyDto
{
    public string? Name { get; set; } = null;
}