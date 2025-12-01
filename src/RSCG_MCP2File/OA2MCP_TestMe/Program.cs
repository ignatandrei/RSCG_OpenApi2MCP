//using Microsoft.AspNetCore.Authentication.Negotiate;
using OA2MCP_TestMe;
using OpenAPISwaggerUI;
using UsefullExtensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddMcpServer()
    .WithToolsFromAssembly()
    .WithStdioServerTransport()
    .WithHttpTransport()
    ;


var app = builder.Build();


    app.MapOpenApi();
    app.UseOpenAPISwaggerUI();
    app.MapMcp();
    
app.MapControllers();

await app.RunAsync();
