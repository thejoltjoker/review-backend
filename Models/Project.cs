namespace Review.Api.Models;

public class Project(string name, string userId)
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // TODO add user roles per project
    public List<User> Users { get; } = [];
    
    public ICollection<Asset> Assets { get; } = new List<Asset>(); 

}