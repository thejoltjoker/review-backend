using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Review.Api.Models.DTOs;
using Review.Api.Services;

namespace Review.Api.Controllers;

[ApiController]
[Route("[controller]")]
// Only bearer tokens for key management
[Authorize(Policy = "BearerOnly")]
public class ApiKeysController : ControllerBase
{
    private readonly IApiKeyService _service;
    private readonly ILogger<ApiKeysController> _logger;

    public ApiKeysController(IApiKeyService service, ILogger<ApiKeysController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApiKeyDto>>> GetAll()
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
            _logger.LogError(e, "Error while fetching ApiKeys");
            return Problem("Something went wrong");
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
            var result = await _service.CreateAsync(userId, data.Name, DateTime.UtcNow.AddDays(90));
            // if (result == null) return BadRequest("Project couldn't be created");

            // TODO Verify key is only visible once after creation.
            return Created(string.Empty, result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating ApiKey");

            return Problem("Something went wrong");
        }
    }

    [HttpDelete]
    [Route("{keyId}")]
    public async Task<ActionResult> Revoke(string keyId)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var result = await _service.RevokeAsync(userId, keyId);
            if (!result) return Problem("Couldn't revoke api key");
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while revoking ApiKey {KeyId}", keyId);
            return Problem("Something went wrong");
        }
    }
}