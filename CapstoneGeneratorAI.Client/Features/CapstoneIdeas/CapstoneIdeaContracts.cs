using System.Text.Json.Serialization;

namespace CapstoneGeneratorAI.Client.Features.CapstoneIdeas;

/// <summary>Industry the generated capstone idea should belong to.</summary>
/// <remarks>
/// This is the client's copy of the API contract. Enum values are sent by number, so the
/// member order must match CapstoneGeneratorAI.Api's copy.
/// </remarks>
public enum Industry
{
    All,
    Business,
    Education,
    Healthcare,
    Technology,
    Entertainment,
    Agriculture,
    Government,
    Cybersecurity,
    Energy
}

/// <summary>Kind of application the generated capstone idea should describe.</summary>
public enum ProjectType
{
    Web_App,
    Mobile_App,
    Desktop_App,
    IoT_App,
    Data_App,
    AI_App,
    All
}

public class IdeaRequest
{
    [JsonPropertyName("industry")]
    public Industry Industry { get; set; }

    [JsonPropertyName("type")]
    public ProjectType Type { get; set; }
}

public class IdeaResponse
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("features")]
    public List<string>? Features { get; set; }
}
