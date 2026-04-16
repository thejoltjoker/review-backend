namespace Review.Api.Models;

public class BaseEntity
{
    // TODO Look into using Guid type or generic
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}