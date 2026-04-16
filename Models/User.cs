using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Review.Api.Models;

public class User : IdentityUser
{
    public ICollection<ApiKey> ApiKeys { get; } = new List<ApiKey>();
    public List<Project> Projects { get; } = [];
    public ICollection<Asset> Assets { get; } = new List<Asset>(); 
    public ICollection<Comment> Comments { get; } = new List<Comment>();

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}