namespace CapstoneGeneratorAI.Api.Features.CapstoneIdeas;

/// <summary>Where to reach Ollama and which model to ask.</summary>
public class OllamaOptions
{
    public const string SectionName = "Ollama";

    /// <summary>
    /// Base address of the Ollama server. Defaults to the host machine as seen from
    /// inside a container; use http://localhost:11434 when running outside Docker.
    /// </summary>
    public string BaseUrl { get; set; } = "http://host.docker.internal:11434";

    public string Model { get; set; } = "llama3.2:1b";
}
