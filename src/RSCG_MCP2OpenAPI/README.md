# RSCG_MCP2OpenAPI

RSCG_MCP2OpenAPI is a .NET Standard 2.0 source generator that , from MCP tools , generates SWAGGER / OPEN API



## Features
- Generates /api/mcp/{className}/{funcName} endpoint for any MCP tools 
- 
## What it generates


For example, given the following MCP tool definition:
```csharp
[McpServerToolType]
[MCP2OpenAPI.AddMCP2OpenApi]
public partial class WeatherTool
{
    [McpServerTool, Description("echoes the UTC date time in sortable format")]
    public string CurrentISODateTime()
    {
        return DateTime.UtcNow.ToString("s");
    }
    [McpServerTool, Description("echoes the string")]
    public string EchoBack(string theString)
    {
        return theString;
    }
```

The generator will produce the following endpoints:
```csharp
internal static partial class WeatherTool_OpenAPI
{
    public static void AddAll_WeatherTool(this Microsoft.AspNetCore.Routing.IEndpointRouteBuilder builder){
Add_CurrentISODateTime(builder);
Add_EchoBack(builder);
Add_Add2Numbers(builder);
Add_MyGetWeatherForCity(builder);
    }



public static void Add_CurrentISODateTime (Microsoft.AspNetCore.Routing.IEndpointRouteBuilder builder){
    
        builder.MapPost("/api/mcp/WeatherTool/CurrentISODateTime",([Microsoft.AspNetCore.Mvc.FromServices]OA2MCP_TestMe.WeatherTool toolClass)=>
        toolClass.CurrentISODateTime()
        );

}




    public record rec_EchoBack ( string? theString );
    
public static void Add_EchoBack (Microsoft.AspNetCore.Routing.IEndpointRouteBuilder builder){
    
        builder.MapPost("/api/mcp/WeatherTool/EchoBack",([Microsoft.AspNetCore.Mvc.FromServices]OA2MCP_TestMe.WeatherTool toolClass,[Microsoft.AspNetCore.Mvc.FromBody]rec_EchoBack  value)=>
        toolClass.EchoBack(value.theString)
        );

}

```


## How to use

Add RSCG_MCP2OpenAPI as a NuGet package to your project. 
The generator will automatically run at build time and generate the necessary swagger code from tool definition.

Add to your Program.cs or Startup.cs the following line to register the generated endpoints:
```csharp
/// Add MCP Server tool as service
builder.Services.AddTransient<WeatherTool>();
//code
//app.MapGet("/", () => "todo");
//assuming your MCP tool class is named WeatherTool
app.AddAll_WeatherTool();
```

## License
This project is licensed under the MIT License. 

## Repository

For more information, visit the [GitHub repository](https://github.com/ignatandrei/RSCG_OpenApi2MCP).
