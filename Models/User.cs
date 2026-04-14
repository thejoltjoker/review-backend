using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Review.Api.Models;

public class User : IdentityUser
{
    public ApiKey? ApiKey { get; set; } 

    public ICollection<ProjectUser> ProjectUsers { get; } = [];
    [NotMapped]
    public IEnumerable<Project> Projects => ProjectUsers.Select(pu => pu.Project);
    
    public ICollection<Asset> Assets { get; } = new List<Asset>(); // Collection navigation containing dependents
    public ICollection<Comment> Comments { get; } = new List<Comment>(); // Collection navigation containing dependents
}