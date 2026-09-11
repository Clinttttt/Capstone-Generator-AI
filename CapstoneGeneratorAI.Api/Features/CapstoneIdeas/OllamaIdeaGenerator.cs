using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace CapstoneGeneratorAI.Api.Features.CapstoneIdeas;

/// <summary>Asks Ollama for a capstone idea and maps the reply onto <see cref="IdeaResponse"/>.</summary>
public class OllamaIdeaGenerator(HttpClient http, IOptions<OllamaOptions> options)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly OllamaOptions _options = options.Value;

    public async Task<IdeaResponse?> GenerateAsync(
        Industry industry,
        ProjectType type,
        CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            model = _options.Model,
            messages = new[]
            {
                new { role = "user", content = PromptBuilder.Build(industry, type) }
            },
            stream = false,
            // Ask Ollama to constrain its reply to the shape of IdeaResponse.
            format = new
            {
                type = "object",
                properties = new
                {
                    title = new { type = "string" },
                    description = new { type = "string" },
                    features = new
                    {
                        type = "array",
                        items = new { type = "string" }
                    }
                },
                required = new[] { "title", "description", "features" }
            }
        };

        var response = await http.PostAsJsonAsync("api/chat", payload, cancellationToken);
        if (!response.IsSuccessStatusCode)
            return null;

        var chat = await response.Content.ReadFromJsonAsync<OllamaChatResponse>(
            JsonOptions, cancellationToken);

        var content = chat?.Message?.Content;
        if (string.IsNullOrWhiteSpace(content))
            return null;

        // The model returns the idea as a JSON string inside the chat message.
        return JsonSerializer.Deserialize<IdeaResponse>(content, JsonOptions);
    }

    private sealed class OllamaChatResponse
    {
        [JsonPropertyName("message")]
        public OllamaChatMessage? Message { get; set; }
    }

    private sealed class OllamaChatMessage
    {
        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }
}
