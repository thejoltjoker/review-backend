using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Review.Api.Models.DTOs;
using Review.Api.Services;

namespace Review.Api.Controllers;

[ApiController]
[Route("[controller]")]
// Only bearer tokens for key management
[Authorize]
public class ApiKeysController : ControllerBase
{
    private readonly IApiKeyService _service;

    public ApiKeysController(IApiKeyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiKeyDto>> GetAll()
    {
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var result = await _service.GetAllAsync(userId);
            // if (result == null) return BadRequest("Project couldn't be created");

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }
    [HttpPost]
    public async Task<ActionResult<ApiKeyDto>> Create([FromBody] CreateApiKeyDto data)
    {
        try
        {
            // TODO Improve error handling
            // TODO Set expiration
            // TODO Revoke all other api keys
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var result = await _service.CreateAsync(userId, data.Name);
            // if (result == null) return BadRequest("Project couldn't be created");

            // TODO Verify key is only visible once after creation.
            return Created(string.Empty, result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }
}