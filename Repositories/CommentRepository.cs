using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Review.Api.Contexts;
using Review.Api.Models;

namespace Review.Api.Repositories;

public class CommentRepository(ApplicationDbContext context) : ICommentRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Comment>> GetAllByAssetIdAsync(string userId, string assetId)
    {
        // TODO Check if user has access to project
        return await _context.Comments.AsNoTracking().Where(comment => comment.AssetId == assetId).ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(string userId, string commentId)
    {
        return await _context.Comments.AsNoTracking().FirstOrDefaultAsync(comment => comment.Id == commentId);
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        return comment;
    }

    public void Update(Comment comment)
    {
        _context.Comments.Update(comment);
    }

    public void Delete(Comment comment)
    {
        _context.Comments.Remove(comment);
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}