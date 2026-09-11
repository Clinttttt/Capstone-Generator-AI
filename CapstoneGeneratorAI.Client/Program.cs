using CapstoneGeneratorAI.Client.Components;
using CapstoneGeneratorAI.Client.Features.CapstoneIdeas;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Capstone ideas feature: typed client pointed at the CapstoneGeneratorAI API.
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7094";

builder.Services.AddHttpClient<CapstoneIdeaApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl.TrimEnd('/') + "/");
    // The API waits on a local model, so allow it plenty of time to answer.
    client.Timeout = TimeSpan.FromMinutes(5);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. See https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
