
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddMcpServer()
    .WithToolsFromAssembly()
    .WithStdioServerTransport()
    .WithHttpTransport()
    ;
builder.Services.AddTransient<WeatherTool>();

var app = builder.Build();

    app.MapOpenApi();
    app.UseOpenAPISwaggerUI();
    app.MapMcp();
app.MapGet("/", () => "todo");
app.MapControllers();
app.AddAll_WeatherTool();
await app.RunAsync();
