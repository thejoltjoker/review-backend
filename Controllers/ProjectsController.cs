using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Review.Api.Models;
using Review.Api.Models.DTOs;
using Review.Api.Services;

namespace Review.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Policy = "ApiKeyOrUser")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectsController(IProjectService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _service.GetAllAsync(userId);

        return Ok(result);
    }

    [HttpGet]
    [Route("{projectId}")]
    public async Task<ActionResult<ProjectWithAssetsDto>> GetById(string projectId)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        ProjectWithAssetsDto? result = await _service.GetByIdAsync(userId, projectId);
        if (result == null) return NotFound($"Project {projectId} not found");

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Project>> Create([FromBody] CreateProjectDto data)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _service.CreateAsync(userId, data);

        return CreatedAtAction(nameof(GetById), new { projectId = result.Id }, result);
    }

    [HttpPut]
    [Route("{projectId}")]
    public async Task<ActionResult> Update(string projectId, [FromBody] UpdateProjectDto data)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _service.UpdateAsync(userId, projectId, data);
        if (!result) return NotFound($"Project {projectId} not found");

        return NoContent();
    }

    [HttpDelete]
    [Route("{projectId}")]
    public async Task<ActionResult> Delete(string projectId)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _service.DeleteAsync(userId, projectId);
        if (!result) return NotFound($"Couldn't find project {projectId}");

        return NoContent();
    }
}