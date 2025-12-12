# RSCG_MCP2File

[![NuGet](https://img.shields.io/nuget/v/RSCG_MCP2File.svg)](https://www.nuget.org/packages/RSCG_MCP2File)

RSCG_MCP2File is a .NET Standard 2.0 source generator that generates MCP (Microsoft Code Platform) tools that generates exports to file from each MCP function defined 


## Features
- Generates tool from another 
- 
## Usage
Add RSCG_MCP2File as a NuGet package to your project. 

Add the following attribute to your MCP function to generate a file export tool:
```csharp
[MCP2File.AddMCPExportToFile]
[McpServerToolType]
public partial class WeatherTool
{
```

In the program.cs file, register the generated tool:
```csharp
builder.Services.AddMcpServer()
    .WithToolsFromAssembly()
```

## How it works

For each function that returns a value, a corresponding tool will be generated that exports the result to a file.
It assumes that the result is either a byte array, a string, or an object that can be converted to a string.

For example
```csharp
[McpServerToolType]
[MCP2File.AddMCPExportToFile]
public partial class WeatherTool
{
    [McpServerTool, Description("Returns the current weather for a specific city")]
    public string MyGetWeatherForCity(string cityName)
    { 
```

it generates 

```csharp
  [global::ModelContextProtocol.Server.McpServerTool]
        [global::System.ComponentModel.Description("calls the MyGetWeatherForCity and saves the result to a file . Investigate Success parameter from result and, if false, see the ErrorMessage ")]
        public async Task<MCP2File.ResultWriteToFile> MyGetWeatherForCityExportToFile(string cityName, [global::System.ComponentModel.Description("please use full file path. Do NOT use relative path ")]string exportToFile)
        {
        try{
            dynamic result = MyGetWeatherForCity(cityName);
            if (result is byte[] bytes)
            {
                await File.WriteAllBytesAsync(exportToFile, bytes);
            }
            else if (result is string str)
            {
                await File.WriteAllTextAsync(exportToFile, str);
            }
            else
            {
                await File.WriteAllTextAsync(exportToFile, result?.ToString() ?? string.Empty);
            }
            return MCP2File.ResultWriteToFile.FromSuccess();
        }
        catch (Exception ex)
        {
             return MCP2File.ResultWriteToFile.FromError(ex.Message);
            
        }
        }
```

The ResultWriteToFile class is defined as:
```csharp
public class ResultWriteToFile
{
    public bool  Success { get; set; }
    public string? ErrorMessage { get; set; }
    public static ResultWriteToFile FromSuccess() => new ResultWriteToFile { Success = true};
    public static ResultWriteToFile FromError(string errorMessage) => new ResultWriteToFile { Success = false, ErrorMessage = errorMessage };
}
```
The AI should investigate the Success parameter from the result and, if false, see the ErrorMessage.

## License
This project is licensed under the MIT License. 

## Repository
For more information, visit the [GitHub repository](https://github.com/ignatandrei/RSCG_OpenApi2MCP).
