//using Microsoft.AspNetCore.Authentication.Negotiate;
using OA2MCP_TestMe;
using OpenAPISwaggerUI;
using UsefullExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
//   .AddNegotiate();

//builder.Services.AddAuthorization(options =>
//{
//    // By default, all incoming requests will be authorized according to the default policy.
//    options.FallbackPolicy = options.DefaultPolicy;
//});


builder.Services.AddMcpServer().WithToolsFromAssembly();

builder.Services.AddHostedService<myServerAddress>();

var app = builder.Build();

app.MapDefaultEndpoints();
//app.MapUsefullAll();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenAPISwaggerUI();
    app.MapMcpSse();
    
}
app.MapGet("/EchoWorld", (string id) => $"Hello World {id}!")
    .WithSummary("summary Echo the id")
    .WithDescription("description Echo the id")
    ;

app.MapPost("/SaveData", async (WeatherForecast weatherForecast) =>
{
    //simulate save to database
    await Task.Delay(1000);
    return Random.Shared.Next(0, 100);
})
    .WithSummary("save weather forecast")
    .WithDescription("save weather forecast")
    ;
//app.UseHttpsRedirection();

HttpClient client = new HttpClient();
//client.PostAsJsonAsync()
//app.UseAuthorization();

app.MapControllers();

app.Run();
