using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Review.Api.Models;
using Review.Api.Models.DTOs;
using Review.Api.Services;

namespace Review.Api.Controllers;

// TODO add authorization
[ApiController]
[Route("[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;
    private readonly UserManager<User> _userManager;

    public ProjectsController(UserManager<User> userManager, IProjectService service)
    {
        _service = service;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
    {
        // TODO implement global exception handler
        try
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<ProjectWithAssetsDto>> GetById(string id)
    {
        // TODO implement global exception handler
        try
        {
            ProjectWithAssetsDto? result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound($"Project {id} not found");
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
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
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();
            var result = await _service.CreateAsync(currentUser.Id, data);
            // if (result == null) return BadRequest("Project couldn't be created");

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> Update(string id, Project project)
    {
        // TODO implement global exception handler
        // TODO validate data
        try
        {
            var result = await _service.UpdateAsync(id, project);
            if (!result) return BadRequest($"Couldn't update project {id}");
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        // TODO implement global exception handler
        try
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return BadRequest($"Couldn't delete project {id}");
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }
}