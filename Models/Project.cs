using System.ComponentModel.DataAnnotations.Schema;

namespace Review.Api.Models;

public class Project : BaseEntity
{
    public string Name { get; set; }

    public List<User> Users { get; } = [];
    
    public ICollection<Asset> Assets { get; } = new List<Asset>();

    
}