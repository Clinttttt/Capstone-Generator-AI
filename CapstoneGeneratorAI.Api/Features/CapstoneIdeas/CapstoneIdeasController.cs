using Microsoft.AspNetCore.Mvc;

namespace CapstoneGeneratorAI.Api.Features.CapstoneIdeas;

[ApiController]
[Route("api/ideas")]
public class CapstoneIdeasController(OllamaIdeaGenerator generator) : ControllerBase
{
    /// <summary>Generates a single capstone idea for the requested industry and project type.</summary>
    [HttpPost("ask")]
    [ProducesResponseType(typeof(IdeaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult<IdeaResponse>> AskAsync(
        [FromBody] IdeaRequest request,
        CancellationToken cancellationToken)
    {
        var idea = await generator.GenerateAsync(request.Industry, request.Type, cancellationToken);

        if (idea is null)
            return StatusCode(StatusCodes.Status502BadGateway, "The model did not return an idea.");

        return Ok(idea);
    }
}
