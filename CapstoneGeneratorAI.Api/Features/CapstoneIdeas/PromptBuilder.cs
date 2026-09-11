namespace CapstoneGeneratorAI.Api.Features.CapstoneIdeas;

/// <summary>Builds the natural-language prompt sent to the language model.</summary>
public static class PromptBuilder
{
    public static string Build(Industry industry, ProjectType type)
    {
        var industryText = industry switch
        {
            Industry.All => "a random field",
            Industry.Business => "Business",
            Industry.Education => "Education",
            Industry.Healthcare => "Healthcare",
            Industry.Technology => "Technology",
            Industry.Entertainment => "Entertainment",
            Industry.Agriculture => "Agriculture",
            Industry.Government => "Government",
            Industry.Cybersecurity => "Cybersecurity",
            Industry.Energy => "Energy",
            _ => "any industry of your choice"
        };

        var typeText = type switch
        {
            ProjectType.Web_App => "Web Application",
            ProjectType.Mobile_App => "Mobile Application",
            ProjectType.Desktop_App => "Desktop Application",
            ProjectType.IoT_App => "IoT Application",
            ProjectType.Data_App => "Data Application",
            ProjectType.AI_App => "AI Application",
            _ => "any type of application"
        };

        return $"Generate exactly 1 unique capstone project title and 5 features of it. " +
               $"of {industryText} and the type of {typeText} Return only the title, a clear " +
               $"description and make sure to have minimum of 5 features with 2-3 words each.";
    }
}
