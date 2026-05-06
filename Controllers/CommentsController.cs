using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Review.Api.Models;
using Review.Api.Models.DTOs;
using Review.Api.Services;

namespace Review.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "ApiKeyOrUser")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _service;

    public CommentsController(ICommentService service)
    {
        _service = service;
    }

    // TODO Get all by asset id
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<CommentDto>>> GetAll()
    // {
    //     string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    //     if (string.IsNullOrEmpty(userId)) return Unauthorized();
    //
    //     IEnumerable<CommentDto> result = await _service.GetAllByAssetIdAsync(userId);
    //     return Ok(result);
    // }


    [HttpGet]
    [Route("{commentId}")]
    public async Task<ActionResult<CommentDto>> GetById(string commentId)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        CommentDto? result = await _service.GetByIdAsync(userId, commentId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentDto data)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        CommentDto result = await _service.CreateAsync(userId, data);
        return CreatedAtAction(
            nameof(GetById),
            new { commentId = result.Id },
            result
        );
    }

    [HttpPut]
    [Route("{commentId}")]
    public async Task<ActionResult> Update([FromRoute] string commentId, [FromBody] UpdateCommentDto data)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        EntityStatus result = await _service.UpdateAsync(userId, commentId, data);
        if (result == EntityStatus.Updated || result == EntityStatus.NoChanges) return NoContent();
        if (result == EntityStatus.NotFound) return NotFound();
        if (result == EntityStatus.Forbidden) return Forbid();
        if (result == EntityStatus.InvalidReference)
        {
            return UnprocessableEntity(new
            {
                message = "Referenced entity does not exist or is inaccessible."
            });
        }

        return Problem("Something went wrong");
    }


    [HttpDelete]
    [Route("{commentId}")]
    public async Task<ActionResult> Delete(string commentId)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _service.DeleteAsync(userId, commentId);
        if (result == EntityStatus.NotFound) return NotFound();
        if (result == EntityStatus.Deleted) return NoContent();
        return NoContent();
    }
}