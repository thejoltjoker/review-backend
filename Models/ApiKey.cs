using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Review.Api.Services;

namespace Review.Api.Models;

[Index(nameof(KeyId), IsUnique = true)]
public class ApiKey : BaseEntity
{
    public ApiKey(string userId, string keyHash, string? keyId, string? name)
    {
        UserId = userId;
        Name = name;
        KeyId = keyId ?? ApiKeyGenerator.GenerateApiKeyId();
        KeyHash = keyHash;
    }


    [MaxLength(128)]
    public string? Name { get; set; }

    [MaxLength(128)]
    public string KeyId { get; set; }
    [MaxLength(128)]
    public string KeyHash { get; set; }

    public DateTime? ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    [MaxLength(128)]
    public string UserId { get; set; }

    public User User { get; set; } = null!;
}