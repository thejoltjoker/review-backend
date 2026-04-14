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
            IEnumerable<Project> projects = await _service.GetAllAsync();
            return Ok(projects);
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
            Project? project = await _service.GetByIdAsync(id);
            if (project == null) return NotFound($"Project {id} not found");
            return Ok(project);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }
}