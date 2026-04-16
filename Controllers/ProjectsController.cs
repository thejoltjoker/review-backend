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
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(IProjectService service, ILogger<ProjectsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
    {
        // TODO implement global exception handler
        try
        {
            // var currentUser = await _userManager.GetUserAsync(User);
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var result = await _service.GetAllAsync(userId);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while fetching all projects");
            return Problem("Something went wrong");
        }
    }

    [HttpGet]
    [Route("{projectId}")]
    public async Task<ActionResult<ProjectWithAssetsDto>> GetById(string projectId)
    {
        // TODO implement global exception handler
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            ProjectWithAssetsDto? result = await _service.GetByIdAsync(userId, projectId);
            if (result == null) return NotFound($"Project {projectId} not found");
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while fetching project {ProjectId}", projectId);
            return Problem("Something went wrong");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Project>> Create([FromBody] CreateProjectDto data)
    {
        // TODO implement global exception handler
        // TODO validate data

        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var result = await _service.CreateAsync(userId, data);
            // if (result == null) return BadRequest("Project couldn't be created");

            return CreatedAtAction(nameof(GetById), new { projectId = result.Id }, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating project");
            return Problem("Something went wrong");
        }
    }

    [HttpPut]
    [Route("{projectId}")]
    public async Task<ActionResult> Update(string projectId, [FromBody] UpdateProjectDto data)
    {
        // TODO implement global exception handler
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            
            var result = await _service.UpdateAsync(userId, projectId, data);
            if (!result) return NotFound($"Project {projectId} not found");
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating project {ProjectId}", projectId);
            return Problem("Something went wrong");
        }
    }

    [HttpDelete]
    [Route("{projectId}")]
    public async Task<ActionResult> Delete(string projectId)
    {
        // TODO implement global exception handler
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var result = await _service.DeleteAsync(userId, projectId);
            if (!result) return BadRequest($"Couldn't delete project {projectId}");
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting project {ProjectId}", projectId);
            return Problem("Something went wrong");
        }
    }
}