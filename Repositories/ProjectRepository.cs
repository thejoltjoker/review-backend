using Microsoft.EntityFrameworkCore;
using Review.Api.Contexts;
using Review.Api.Models;

namespace Review.Api.Repositories;

public class ProjectRepository(AppDbContext context) : IProjectRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<Project>> GetAllAsync() =>
        await _context.Projects
            .Include(project => project.Assets)
            .ToListAsync();

    public async Task<Project?> GetByIdAsync(string id) =>
        await _context.Projects
            .Include(project => project.Assets)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Project project) => await _context.Projects.AddAsync(project);

    public void Update(Project project) => _context.Projects.Update(project);

    public void Delete(Project project) => _context.Projects.Remove(project);

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}