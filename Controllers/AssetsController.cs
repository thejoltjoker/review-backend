using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Review.Api.Models;
using Review.Api.Models.DTOs;
using Review.Api.Services;

namespace Review.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "ApiKeyOrUser")]
public class AssetsController : ControllerBase
{
    private readonly IAssetService _service;

    public AssetsController(IAssetService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AssetDto>>> GetAll()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        IEnumerable<AssetDto> result = await _service.GetAllAsync(userId);
        return Ok(result);
    }


    [HttpGet]
    [Route("{assetId}")]
    public async Task<ActionResult<AssetDto>> GetById(string assetId)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        AssetDto? result = await _service.GetByIdAsync(userId, assetId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AssetDto>> Create([FromBody] CreateAssetDto data)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        AssetDto result = await _service.CreateAsync(userId, data);
        return CreatedAtAction(
            nameof(GetById),
            new { assetId = result.Id },
            result
        );
    }

    [HttpPut]
    [Route("{assetId}")]
    public async Task<ActionResult> Update([FromRoute] string assetId, [FromBody] UpdateAssetDto data)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _service.UpdateAsync(userId, assetId, data);
        if (!result) return NotFound();
        return NoContent();
    }
    
    
    [HttpDelete]
    [Route("{assetId}")]
    public async Task<ActionResult> Delete(string assetId)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _service.DeleteAsync(userId, assetId);
        // TODO Add more variation to error handling, i.e. unauthorized (because not project owner)
        if (result == EntityStatus.NotFound) return NotFound();
        if (result == EntityStatus.Deleted) return NoContent();
        return NoContent();
    }
}