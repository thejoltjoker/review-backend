using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Review.Api.Contexts;
using Review.Api.Models;

namespace Review.Api.Repositories;

public class ProjectRepository(ApplicationDbContext context) : IProjectRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects
            .Include(project => project.Assets)
            .ToListAsync();
    }

    public async Task<List<Project>> GetAllByUserIdAsync(string userId)
    {
        return await _context.Projects
            .Where(p => p.ProjectUsers.Any(u => u.UserId == userId))
            .Include(project => project.Assets)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(string projectId) =>
        await _context.Projects
            .Include(project => project.Assets)
            .Include(project => project.ProjectUsers)
            .ThenInclude(projectUser => projectUser.User)
            .FirstOrDefaultAsync(p => p.Id == projectId);

    public async Task<Project?> GetByIdForUserAsync(string userId, string projectId)
    {
        return await _context.Projects
            .Where(p => p.ProjectUsers.Any(u => u.UserId == userId))
            .Include(project => project.Assets)
            .Include(project => project.ProjectUsers)
            .ThenInclude(projectUser => projectUser.User)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task<bool> ExistsForUserAsync(string userId, string projectId)
    {
        Project? result = await _context.Projects
            .AsNoTracking()
            .Where(project => project.ProjectUsers.Any(projectUser => projectUser.UserId == userId))
            .FirstOrDefaultAsync(project => project.Id == projectId);
        return result != null;
    }

    public async Task<Project> AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        return project;
    }

    public void Update(Project project) => _context.Projects.Update(project);

    public void Delete(Project project) => _context.Projects.Remove(project);

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}