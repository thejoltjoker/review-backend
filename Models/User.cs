using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Review.Api.Models;

public class User : IdentityUser
{
    public ApiKey? ApiKey { get; set; } 

    
    public List<Project> Projects { get; } = [];
    public ICollection<Asset> Assets { get; } = new List<Asset>(); 
    public ICollection<Comment> Comments { get; } = new List<Comment>();
}