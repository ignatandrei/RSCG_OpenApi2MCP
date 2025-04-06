//using Microsoft.AspNetCore.Authentication.Negotiate;
using OpenAPISwaggerUI;

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

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenAPISwaggerUI();
    app.MapMcpSse();
}
app.MapGet("/TestAndrei", (int id) => $"Hello World {id}!");
//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
