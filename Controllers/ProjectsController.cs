using Microsoft.AspNetCore.Mvc;
using Review.Api.Models;
using Review.Api.Services;

namespace Review.Api.Controllers;

// TODO add authorization
[ApiController]
[Route("[controller]")]
public class ProjectsController(IProjectService service) : ControllerBase
{
    private readonly IProjectService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetAll()
    {
        // TODO implement global exception handler
        try
        {
            IEnumerable<Project> result = await _service.GetAllAsync();
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
    public async Task<ActionResult<Project>> GetById(string id)
    {
        // TODO implement global exception handler
        try
        {
            Project? result = await _service.GetByIdAsync(id);
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
    public async Task<ActionResult<Project>> Create(Project project)
    {
        // TODO implement global exception handler
        // TODO validate data
        try
        {
            var result = await _service.CreateAsync(project);
            // if (result == null) return BadRequest("Project couldn't be created");

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, project);
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