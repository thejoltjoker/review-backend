namespace Review.Api.Models;

public class ApiKey
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string UserId { get; set; }
    public User User { get; set; } = null!;
    // TODO implement createdAt and updatedAt
}