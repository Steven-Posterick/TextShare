using Microsoft.AspNetCore.Mvc;
using TextShare.API.Exceptions;
using TextShare.API.Services;
using TextShare.Common.Models.Requests;

namespace TextShare.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TextShareController : ControllerBase
{
    private readonly ITextShareService _textShareService;

    public TextShareController(ITextShareService textShareService)
    {
        _textShareService = textShareService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateText([FromBody] TextCreateRequest textRequest)
    {
        try
        {
            var sharedText = await _textShareService.CreateTextAsync(textRequest);
            return CreatedAtAction(nameof(GetTextDetails), new { id = sharedText.Id }, sharedText);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Error occurred while creating the text.", Details = ex.Message });
        }
    }
    
    [HttpGet("details/{id:guid}")]
    public async Task<IActionResult> GetTextDetails(Guid id)
    {
        try
        {
            var sharedText = await _textShareService.GetTextDetailsAsync(id);

            return Ok(sharedText);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Error occurred while retrieving the text details.", Details = ex.Message });
        }
    }
    
    [HttpPost("{id:guid}")]
    public async Task<IActionResult> GetText(Guid id, [FromBody] TextReadRequest? readRequest = null)
    {
        try
        {
            var sharedText = await _textShareService.GetTextAsync(id, readRequest?.Password);

            return Ok(sharedText);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { Message = "Invalid password." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Error occurred while retrieving the text.", Details = ex.Message });
        }
    }
}