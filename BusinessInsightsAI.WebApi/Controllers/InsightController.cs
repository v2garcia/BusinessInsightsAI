using Microsoft.AspNetCore.Mvc;
using BusinessInsightsAI.Application.Common.Interfaces;

namespace BusinessInsightsAI.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InsightController : ControllerBase
{
    private readonly IInsightOrchestrator _Orchestrator;
    
    public InsightController(IInsightOrchestrator contextOrchestrator)
    {
        _Orchestrator = contextOrchestrator;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] AskRequest request)
    {
        if (string.IsNullOrEmpty(request.Prompt))
        {
            return BadRequest("Prompt is required.");
        }
        var result = await _Orchestrator.AskInsightAsync(request.Prompt);
        return Ok(result);
    }
}
public record  AskRequest(string Prompt);