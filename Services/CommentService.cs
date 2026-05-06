using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Review.Api.Models;
using Review.Api.Models.DTOs;
using Review.Api.Repositories;

namespace Review.Api.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;
    private readonly IAssetRepository _assetRepository;
    private readonly IMapper _mapper;


    public CommentService(ICommentRepository repository, IAssetRepository assetRepository, IMapper mapper)
    {
        _repository = repository;
        _assetRepository = assetRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CommentDto>> GetAllByAssetIdAsync(string userId, string assetId)
    {
        // TODO Add error handling
        var result = await _repository.GetAllByAssetIdAsync(userId, assetId);
        return _mapper.Map<IEnumerable<CommentDto>>(result);
    }

    public async Task<CommentDto?> GetByIdAsync(string userId, string commentId)
    {
        Comment? comment = await _repository.GetByIdAsync(userId, commentId);
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<CommentDto> CreateAsync(string userId, CreateCommentDto data)
    {
        Comment comment = _mapper.Map<Comment>(data);
        comment.UserId = userId;
        await _repository.AddAsync(comment);
        await _repository.SaveAsync();
        return _mapper.Map<CommentDto>(comment);
    }

    public async Task<EntityStatus> UpdateAsync(string userId, string commentId, UpdateCommentDto data)
    {
        Comment? comment = await _repository.GetByIdAsync(userId, commentId);
        if (comment == null) return EntityStatus.NotFound;

        bool hasChanges = false;

        if (data.Content != comment.Content)
        {
            comment.Content = data.Content;
            hasChanges = true;
        }

        
        if (Math.Abs(data.TimestampSeconds - comment.TimestampSeconds) > 0.001f)
        {
            comment.TimestampSeconds = data.TimestampSeconds;
            hasChanges = true;
        }
        
        if (!hasChanges) return EntityStatus.NoChanges;

        _repository.Update(comment);
        await _repository.SaveAsync();
        return EntityStatus.Updated;
    }

    public async Task<EntityStatus> DeleteAsync(string userId, string commentId)
    {
        var comment = await _repository.GetByIdAsync(userId, commentId);
        if (comment == null) return EntityStatus.NotFound;
        _repository.Delete(comment);
        await _repository.SaveAsync();
        return EntityStatus.Deleted;
    }
}