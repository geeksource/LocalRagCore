using LocalRAGCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocalRAGCore.Controllers;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly RagService _ragService;

    public ChatController(RagService ragService)
    {
        _ragService = ragService;
    }

    [HttpGet]
    public async Task<IActionResult> Ask(string question)
    {
        var answer = await _ragService.AskAsync(question);

        return Ok(answer);
    }
}