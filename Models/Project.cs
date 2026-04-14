using System.ComponentModel.DataAnnotations.Schema;

namespace Review.Api.Models;

public class Project
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;


    public ICollection<ProjectUser> ProjectUsers { get; } = [];
    [NotMapped]
    public IEnumerable<User> Users => ProjectUsers.Select(pu => pu.User);

    public ICollection<Asset> Assets { get; } = new List<Asset>();

    public static Project Create(string name, string creatorUserId)
    {
        if (string.IsNullOrWhiteSpace(creatorUserId)) throw new MissingFieldException($"Missing creator user id");
        var project = new Project { Name = name };
        project.AddUser(creatorUserId, ProjectUserRole.Creator);
        return project;
    }

    public void AddUser(string userId, ProjectUserRole role = ProjectUserRole.Viewer)
    {
        // TODO prevent duplicate roles for a user
        ProjectUsers.Add(new ProjectUser
        {
            UserId = userId,
            ProjectId = Id,
            Role = role
        });
    }

    public void RemoveUser(string userId)
    {
        var projectUser = ProjectUsers.FirstOrDefault(pu => pu.UserId == userId);
        if (projectUser != null)
            ProjectUsers.Remove(projectUser);
    }
}