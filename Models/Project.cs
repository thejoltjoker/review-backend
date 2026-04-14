using System.ComponentModel.DataAnnotations.Schema;

namespace Review.Api.Models;

public class Project
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<User> Users { get; } = [];
    
    public ICollection<Asset> Assets { get; } = new List<Asset>();

    
}