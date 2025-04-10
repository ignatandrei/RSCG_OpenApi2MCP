# RSCG_OpenApi2MCP
.NET Open API to MCP

# How it works

The Microsoft.Extensions.ApiDescription.Server generates on build the OpenAPI spec for the API. This is then used to generate the MCP files.
The OpenAPI spec is generated in the `obj` folder. The MCP files are generated in the `$(BaseIntermediateOutputPath)\GX\` folder.


# Installation 

## Mandatory NuGet packages
Add this to the csproj

```xml
<ItemGroup>
     <PackageReference Include="RSCG_OpenApi2MCP" Version="9.2025.410.706" OutputItemType="Analyzer"  />
</ItemGroup>

```

Ensure also that you have the following in your csproj:
```xml
 <PropertyGroup>	
    <PackageReference Include="ModelContextProtocol" Version="0.1.0-preview.4" />
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="9.0.3" />
    <PackageReference Include="Microsoft.Extensions.ApiDescription.Server" Version="9.0.3">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>

 </PropertyGroup>
```

This is optional, but allows you to see what is generated for the MCP to work.

```xml
<PropertyGroup>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <CompilerGeneratedFilesOutputPath>$(BaseIntermediateOutputPath)\GX</CompilerGeneratedFilesOutputPath>
</PropertyGroup>
```

## What to add to program.cs

```csharp
builder.Services.AddOpenApi();
builder.Services.AddMcpServer().WithToolsFromAssembly();

builder.Services.AddHostedService<myServerAddress>();
//code

app.MapOpenApi();
app.UseOpenAPISwaggerUI();
app.MapMcpSse();

app.MapGet("/EchoWorld", (string id) => $"Hello World {id}!")
    .WithSummary("summary Echo the id")
    .WithDescription("description Echo the id")
    ;

```

## Important notes

Every time that you want to have mcp files , please build 2 times in order to generate the correct MCP files. The first time the OpenAPI spec is generated, the second time the MCP files are generated. This is because the MCP files are generated from the OpenAPI spec.