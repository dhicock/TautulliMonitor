var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

using HttpClient client = new();
var tautulliEndpoint = builder.Configuration["TAUTULLI_ENDPOINT"];
var tautulliPort = builder.Configuration["TAUTULLI_PORT"];
var tautulliApiKey = builder.Configuration["TAUTULLI_APIKEY"];

string buildEndpoint()
{
    if (tautulliEndpoint.StartsWith("http"))
    {
        return $"{tautulliEndpoint}:{tautulliPort}/api/v2?apikey={tautulliApiKey}";
    }
    return $"http://{tautulliEndpoint}:{tautulliPort}/api/v2?apikey={tautulliApiKey}";
}

app.MapGet("/ping", async () =>
{
    var result = await client.GetAsync($"{buildEndpoint()}&cmd=status");
    return result.StatusCode == System.Net.HttpStatusCode.OK;
});

app.Run();