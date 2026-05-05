using System.Collections.Generic;
using System.Threading.Tasks;
using Review.Api.Models;

namespace Review.Api.Repositories;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllByAssetIdAsync(string userId, string assetId);
    Task<Comment?> GetByIdAsync(string userId, string commentId);
    Task<Comment> AddAsync(Comment comment);
    void Update(Comment comment);
    void Delete(Comment comment);
    Task<int> SaveAsync();
}