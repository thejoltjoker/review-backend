namespace Review.Api.Models;

public class Project
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<User> Users { get; } = [];
    // TODO add user roles per project
    
    public ICollection<Asset> Assets { get; } = new List<Asset>(); 

}