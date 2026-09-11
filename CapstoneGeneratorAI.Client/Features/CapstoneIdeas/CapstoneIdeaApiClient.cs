using System.Net.Http.Json;

namespace CapstoneGeneratorAI.Client.Features.CapstoneIdeas;

/// <summary>Calls the CapstoneGeneratorAI API to fetch a generated idea.</summary>
public class CapstoneIdeaApiClient(HttpClient http, ILogger<CapstoneIdeaApiClient> logger)
{
    public async Task<IdeaResponse?> AskAsync(
        IdeaRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await http.PostAsJsonAsync("api/ideas/ask", request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Idea request failed with status {StatusCode}.", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<IdeaResponse>(cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogWarning(ex, "Could not reach the CapstoneGeneratorAI API.");
            return null;
        }
    }
}
