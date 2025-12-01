# RSCG_MCP2File

RSCG_MCP2File is a .NET Standard 2.0 source generator that generates MCP (Microsoft Code Platform) tools that generates exports to file from each MCP function defined 


## Features
- Generates code from Swagger (OpenAPI) definitions during build
- Integrates with the .NET build process
- Designed for use in MCP tool generation scenarios
- Supports .NET Standard 2.0 for broad compatibility

## Usage
Add RSCG_MCP2File as a NuGet package to your project. The generator will automatically run at build time and generate the necessary MCP tool code from your Swagger definitions.

## Build
This project is configured to emit compiler-generated files to the `GX` directory under the intermediate output path. It uses Roslyn and is compatible with CI/CD environments.

## License
This project is licensed under the MIT License. 

## Repository
For more information, visit the [GitHub repository](https://github.com/ignatandrei/RSCG_OpenApi2MCP).
