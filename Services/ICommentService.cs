using System.Collections.Generic;
using System.Threading.Tasks;
using Review.Api.Models;
using Review.Api.Models.DTOs;

namespace Review.Api.Services;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetAllByAssetIdAsync(string userId, string assetId);
    Task<CommentDto?> GetByIdAsync(string userId, string commentId);
    Task<(EntityStatus Status, CommentDto? Comment)> CreateAsync(string userId, CreateCommentDto data);
    Task<EntityStatus> UpdateAsync(string userId, string commentId, UpdateCommentDto data);
    Task<EntityStatus> DeleteAsync(string userId, string commentId);
}