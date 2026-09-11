using CapstoneGeneratorAI.Api.Features.CapstoneIdeas;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Capstone ideas feature: options plus the typed Ollama client it talks to.
builder.Services.Configure<OllamaOptions>(
    builder.Configuration.GetSection(OllamaOptions.SectionName));

builder.Services.AddHttpClient<OllamaIdeaGenerator>((provider, client) =>
{
    var options = builder.Configuration
        .GetSection(OllamaOptions.SectionName)
        .Get<OllamaOptions>() ?? new OllamaOptions();

    client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
    // Generating on a small local model can take a while on cold start.
    client.Timeout = TimeSpan.FromMinutes(5);
});

// NOTE: the API has no authentication and accepts any origin. Anyone who can reach it
// can spend time on the local model, so keep it off the public internet as-is.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapGet("/", () => "CapstoneGeneratorAI API is running!");

app.Run();
