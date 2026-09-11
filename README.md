# Capstone Generator AI

Generates a capstone project title, description, and five features from a chosen industry
and project type, using a locally hosted **LLaMA 3.2 (1B)** model served by
[Ollama](https://ollama.com).

## How it works

A Blazor Server UI collects the filters and posts them to a small ASP.NET Core API. The
API builds the prompt, asks Ollama for a JSON-shaped reply, and returns it to the UI.

```
Browser → CapstoneGeneratorAI.Client (Blazor Server)
            → CapstoneGeneratorAI.Api (POST /api/ideas/ask)
                → Ollama (POST /api/chat, llama3.2:1b)
```

## Structure

Two runnable projects, each organised by feature (vertical slice) rather than by
technical layer. Everything a feature needs sits in one folder.

```
CapstoneGeneratorAI.Api/
├── Features/CapstoneIdeas/
│   ├── CapstoneIdeasController.cs    POST /api/ideas/ask
│   ├── CapstoneIdeaContracts.cs      Request/response types and enums
│   ├── PromptBuilder.cs              Turns the filters into a prompt
│   ├── OllamaIdeaGenerator.cs        Calls Ollama, parses the reply
│   └── OllamaOptions.cs              Base URL and model name
└── Program.cs

CapstoneGeneratorAI.Client/
├── Features/CapstoneIdeas/
│   ├── Generator.razor               The page at "/"
│   ├── CapstoneIdeaApiClient.cs      Calls the API
│   └── CapstoneIdeaContracts.cs      Client copy of the API contract
├── Components/                       App shell, layout, error page
└── wwwroot/                          Tailwind + daisyUI styles
```

The two projects do not reference each other; they talk over HTTP, so each keeps its own
copy of the small request/response contract. Enum values travel as numbers, so keep the
member order of `Industry` and `ProjectType` identical in both copies.

## Running locally

Requires the .NET 9 SDK and Ollama with the model pulled:

```bash
ollama pull llama3.2:1b
```

Start the API, then the client, in separate terminals:

```bash
dotnet run --project CapstoneGeneratorAI.Api      # https://localhost:7094
dotnet run --project CapstoneGeneratorAI.Client   # https://localhost:7244
```

Open the client URL and press **Generate Ideas**. The first request can take a while
while the model warms up.

## Configuration

The API reads Ollama settings from `appsettings.json` or environment variables:

| Setting | Default | Notes |
| --- | --- | --- |
| `Ollama:BaseUrl` | `http://host.docker.internal:11434` | Use `http://localhost:11434` outside Docker |
| `Ollama:Model` | `llama3.2:1b` | Any model available to your Ollama install |

The client reads `ApiBaseUrl` (default `https://localhost:7094`).

As environment variables, use double underscores: `Ollama__BaseUrl`, `ApiBaseUrl`.

## Docker

Each project has its own Dockerfile:

```bash
docker build -t capstone-api -f CapstoneGeneratorAI.Api/Dockerfile .
docker build -t capstone-web -f CapstoneGeneratorAI.Client/Dockerfile .
```

Inside a container, Ollama on the host is reachable at `host.docker.internal`, which is
why that is the default.

## Styling

Tailwind CSS with daisyUI. `wwwroot/css/site.min.css` is the compiled output and is
committed so the app runs without a Node build. To change styles, edit
`wwwroot/css/site.css` and rebuild:

```bash
cd CapstoneGeneratorAI.Client
npm install
npm run watch
```

## Note

The API has no authentication and allows any origin. It is meant to run locally against
your own Ollama instance; do not expose it publicly as-is.
